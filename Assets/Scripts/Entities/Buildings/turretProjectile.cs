using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turretProjectile : MonoBehaviour
{
    public float damage;

    public LayerMask hitMask;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        //Insert hit logic
        EnemyHealth enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
        if (enemyHealth != null)
        {
            //Debug.Log("Destroyed myself?");
            enemyHealth.TakeDamage(damage);
            Destroy(this.gameObject);
        }

    }
}
