using UnityEngine;

public abstract class Damageable : MonoBehaviour
{
    public HealthComponent hc;
    public Collider2D hitbox;
    public LayerMask hittableLayers;
    protected virtual void Start()
    {
        if (hc.health == 0f)
        {
            hc.health = hc.maxHealth;
        }
    }
    public virtual void Damage(float damage)
    {
        hc.Damage(damage);
    }

    protected virtual void Update()
    {
        if (hc.health <= 0f)
        {
            Destroy(gameObject);
        }
    }
}