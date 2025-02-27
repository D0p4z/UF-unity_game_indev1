using Unity.VisualScripting;
using UnityEngine;

public class BossHealth : EnemyHealth
{
    public void Start()
    {
        base.Start();
        setHealth(getHealth() *50f);
    }
    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage * 2);
        gameObject.GetComponent<SpriteRenderer>().color = new Color((float)base.getHealth() / 255f, 0f, 0f); ;
    }
}
