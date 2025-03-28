using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingGrid : MonoBehaviour
{
    public static PathfindingGrid instance;

    [SerializeField]
    private LayerMask unwalkableMask;
    [SerializeField]
    private Vector2 gridWorldSize;
    [SerializeField]
    private float nodeRadius;

    public Node[,] grid;


    //Will be assigned in PlayerHand
    [HideInInspector]
    public Transform player, buildPlacement;

    private float nodeDiameter;
    private int gridSizeX, gridSizeY;

    public List<EnemyHandler> enemies = new List<EnemyHandler>();

    public int MaxSize {
        get {
            return gridSizeX * gridSizeY;
        }
    }
    private void Awake() {
        transform.position = Vector3.zero;

        instance = this;
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();
    }

    //Shitty solution to try and untangle enemies, as they sometimes get stuck walking into each other
    /*
    int i;
    private void Update() {
        if (i >= enemies.Count) {
            i = 0;
        }
        enemies[i].refreshPath = true;
    }*/

    private void CreateGrid() {
        grid = new Node[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.up * gridWorldSize.y / 2;

        for (int x = 0; x < gridSizeX; x++) {
            for (int y = 0; y < gridSizeY; y++) {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.up * (y * nodeDiameter + nodeRadius);

                bool walkable = !Physics2D.OverlapCircle(worldPoint, nodeRadius - .2f, unwalkableMask);
                grid[x, y] = new Node(walkable, worldPoint, x, y);
            }
        }
    }

    public bool CreateBuilding(GameObject building) {
        Node node = NodeFromWorldPoint(buildPlacement.position);
        if (node.worldPosition.y == 0.5f && (node.worldPosition.x == 0.5 || node.worldPosition.x == -0.5f))
            return false;
        Node playerNode = NodeFromWorldPoint(player.position);
        if (node != playerNode&& node.building == null && node.walkable) {
            node.building = Instantiate(building, node.worldPosition, Quaternion.identity);
            //For the sake of a cleaner hierarchy
            node.building.transform.parent = transform;

            //Clear path for all enemies if a building is placed => they need to find a new path
            enemies.ForEach(enemy => enemy.refreshPath = true);
            return true;
        }
        return false;
    }

    public bool RemoveBuilding(Vector2 removePosition) {
        Node node = NodeFromWorldPoint(removePosition);
        if (node.building != null) {
            Destroy(node.building);
            node.building = null;

            //Clear path for all enemies if a building is removed => they need to find a new path
            enemies.ForEach(enemy => enemy.refreshPath = true);

            return true;
        }
        return false;
    }

    public Node NodeFromWorldPoint(Vector3 worldPosition) {
        float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPosition.y + gridWorldSize.y / 2) / gridWorldSize.y;
        
        int x = Mathf.FloorToInt(Mathf.Clamp((gridSizeX) * percentX, 0, gridSizeX - 1));
        int y = Mathf.FloorToInt(Mathf.Clamp((gridSizeY) * percentY, 0, gridSizeY - 1));
        //int y = Mathf.RoundToInt((gridSizeY) * percentY) + 1;


        return grid[x, y];
    }

    public List<Node> GetNeighbours(Node node) {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++) {
            for (int y = -1; y <= 1; y++) {
                if (x == 0 && y == 0)
                    continue;

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY) {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbours;
    }
    //If building is below max health
    public bool RepairBuilding(GameObject building)
    {
        Node node = NodeFromWorldPoint(buildPlacement.position);
        if (node.worldPosition.y == 0.5f && (node.worldPosition.x == 0.5 || node.worldPosition.x == -0.5f))
            return false;
        if (node.building != null&& node.building.GetComponent<BuildingHealth>().currentHealth < node.building.GetComponent<BuildingHealth>().buildingScriptableObject.buildingHealth)
        {
            node.building.GetComponent<BuildingHealth>().currentHealth = node.building.GetComponent<BuildingHealth>().buildingScriptableObject.buildingHealth;
            return true;
        }
        return false;
    }
    //If building is upgradeable (at maximum health), call upgrade
    //pillows added as argument to check if the cost can be paid (still pays cost in PlayerHand)
    //Returns the repair cost if upgraded, 0 otherwise
    public int UpgradeBuilding(GameObject building, int pillows)
    {
        Node node = NodeFromWorldPoint(buildPlacement.position);
        if (node.building != null
            &&node.building.GetComponent<BuildingHealth>().currentHealth == node.building.GetComponent<BuildingHealth>().buildingScriptableObject.buildingHealth
            &&pillows>=node.building.GetComponent<BuildingHealth>().upgradeCost+1)
        {
            node.building.GetComponent<BuildingHealth>().Upgrade();
            Debug.Log(node.building.GetComponent<BuildingHealth>().upgradeCost);
            return node.building.GetComponent<BuildingHealth>().upgradeCost;
        }
        return 0;
    }

    public List<Node> path;
    public void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, gridWorldSize.y, 0));
        if (grid != null) {
            Node playerNode = NodeFromWorldPoint(player.position);
            Node buildNode = NodeFromWorldPoint(buildPlacement.position);
            foreach (Node n in grid) {
                Gizmos.color = n.walkable && n.building == null ? Color.white : Color.red;
                if (playerNode == n) {
                    Gizmos.color = Color.cyan;
                }
                else if (buildNode == n) {
                    Gizmos.color = new Color(1, 0.5f, 0);
                }
                if (path != null)
                    if (path.Contains(n))
                        Gizmos.color = Color.black;
                Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeRadius * 2 * 0.4f));
            }
        }
    }
}
