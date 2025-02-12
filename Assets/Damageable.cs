using UnityEngine;

public abstract class Damageable : MonoBehaviour
{
    public HealthComponent hc;
    public Collider2D hitbox;
    public LayerMask hittableLayers;
    void Start()
    {
        if (hc.health == 0f)
        {
            hc.health = hc.maxHealth;
        }
    }
    public void Damage(float damage)
    {
        hc.Damage(damage);
    }

    public void Update()
    {
        if (hc.health <= 0f)
        {
            Destroy(gameObject);
        }
    }
}