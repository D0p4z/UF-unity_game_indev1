using UnityEngine;

public class BuildingHealth : MonoBehaviour
{
    public BuildingScriptableObject buildingScriptableObject;
    //Used in repairing: increments with each upgrade
    public int upgradeCost;
    [HideInInspector]
    public int currentHealth;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        upgradeCost= 1;
        currentHealth = buildingScriptableObject.buildingHealth;
    }

    public bool TakeDamage(int damage) {
        currentHealth -= damage;
        if (currentHealth <= 0) {
            print("Building destroyed");
            PathfindingGrid.instance.RemoveBuilding(transform.position);
            return true;
        }
        return false;
    }
    //Improves stats: if it's a wall, significantly increases health
    //if it's a turret, moderately increases health and range
    public void Upgrade()
    {
        upgradeCost++;
        if (gameObject.GetComponent<basicTurret>() != null)
        {
            currentHealth *= 6;
            currentHealth /= 5;
            gameObject.GetComponent<basicTurret>().range++;
            Debug.Log("Upgraded Turret");
        }
        else
        {
            currentHealth *= 3;
            currentHealth /= 2;
            Debug.Log("Upgraded Wall");
        }
    }
}
