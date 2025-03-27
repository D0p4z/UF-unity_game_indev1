using Unity.VisualScripting;
using UnityEngine;

public class IceProjectile : turretProjectile
{
    [SerializeField] float freezeTime;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //Freezes the enemy,
    new public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<basicEnemy>()!=null)
            collision.GetComponent<basicEnemy>().freeze(freezeTime);
        base.OnTriggerEnter2D(collision);
    }
}
