using UnityEngine;

public abstract class Damageable : MonoBehaviour
{
    public HealthComponent hc;
    public Collider2D hitbox;
    
    public SpriteRenderer spr;
    Material normal, white;

    protected virtual void Awake()
    {
        normal = Resources.Load<Material>("Materials/Normal");
        white = Resources.Load<Material>("Materials/White");
    }

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
        spr.material = white;
        CancelInvoke("Unwhite");
        Invoke("Unwhite", 0.06f);
        Debug.Log("tried to damage");
    }
    
    void Unwhite()
    {
        spr.material = normal;
    }

    protected virtual void Update()
    {
        if (hc.health <= 0f)
        {
            Destroy(gameObject);
        }
    }
}