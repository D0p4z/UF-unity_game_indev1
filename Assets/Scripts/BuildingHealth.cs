using UnityEngine;

public class BuildingHealth : MonoBehaviour
{
    public BuildingScriptableObject buildingScriptableObject;
    //Used in repairing to determine if pillows must be spent to build
    public bool toSpend=true;
    [HideInInspector]
    public int currentHealth;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        toSpend = true;
        currentHealth = buildingScriptableObject.buildingHealth;
    }

    public bool TakeDamage(int damage) {
        toSpend = false;
        currentHealth -= damage;
        if (currentHealth <= 0) {
            print("Building destroyed");
            PathfindingGrid.instance.RemoveBuilding(transform.position);
            return true;
        }
        return false;
    }
}
