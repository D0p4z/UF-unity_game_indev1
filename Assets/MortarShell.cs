using UnityEngine;

public class MortarShell : turretProjectile
{
    [SerializeField] float timer;
    [SerializeField] GameObject explosion;
    private bool detonated;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timer = 4f;
        detonated = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (detonated)
        {
            Destroy(gameObject);
            Destroy(this);
        }
        timer -= Time.deltaTime;
        if (timer <= 0f) Detonate();
    }
    new public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<basicEnemy>()!=null)
            Detonate();
    }
    void Detonate()
    {
        GameObject g = GameObject.Instantiate(explosion, transform.position,Quaternion.identity);
        g.GetComponent<ExplosionScript>().damage = damage;
        detonated = true;
    }
}
