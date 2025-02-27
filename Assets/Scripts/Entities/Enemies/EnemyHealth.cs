using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyHealth : MonoBehaviour
{

    //Health Values
    float health;

    static float maxHealth = 5;

    [SerializeField]
    GameObject destroyParticle, pillow;

    [SerializeField]
    float minPillows, maxPillows, spawnRange;

    public void Start()
    {
        health = maxHealth;
        maxHealth *= 1.01f;
    }
    //Now virtual
    public virtual void TakeDamage(float damage)
    {
        health -= damage;

        if(health < 0)
        {
            if(destroyParticle != null)
            {
                GameObject particle = Instantiate(destroyParticle, transform.position, Quaternion.identity);
                Destroy(particle, 5);
            }

            spawnPillows();

            Destroy(this.gameObject);
            return;
        }
    }

    private void spawnPillows()
    {
        float amount = Mathf.RoundToInt(Random.Range(minPillows, maxPillows));

        for (int i = 0; i < amount; i++)
        {
            Vector2 directionPos = new Vector2(Random.Range(-1,1), Random.Range(-1,1));
            directionPos.Normalize();

            Vector3 spawnPosition = directionPos * spawnRange;
            spawnPosition.z = 0;

            Instantiate(pillow, transform.position + spawnPosition, Quaternion.identity);
        }
    }
    public float getHealth() { return health; }
    public void setHealth(float f) { health = f; }
}
