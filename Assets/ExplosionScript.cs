using Unity.VisualScripting;
using UnityEngine;

public class ExplosionScript : MonoBehaviour
{
    public float damage;
    //deletes self when timer expires
    private float timer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timer = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            Destroy(gameObject);
            Destroy(this);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Deals damage to each enemy in explosion radius
        GameObject g = collision.gameObject;
        if (g.GetComponent<EnemyHealth>() != null) {
            float distanceSquared = Mathf.Pow(g.transform.position.x - transform.position.x, 2) + Mathf.Pow(g.transform.position.y - transform.position.y, 2);
            //Sets maximum damage, avoiding division by 0
            if (distanceSquared < 0.1) distanceSquared = 0.1f;
            //Deals damage inversely proportional to distance
            g.AddComponent<EnemyHealth>().TakeDamage(damage/(distanceSquared));
        }
    }
}
