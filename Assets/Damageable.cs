using UnityEngine;

public abstract class Damageable : MonoBehaviour
{
    public HealthComponent hc;
    public Collider2D hitbox;
    public float squashSpeed = 10f;
    public SpriteRenderer spr;
    Material normal, white;
    public GameObject corpse, damageIndicator;

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
        spr.transform.localScale = new Vector3(0.8f, 1.2f, 1f);
        SpawnDamageIndicator(damage);
    }

    public void SpawnDamageIndicator(float damage)
    {
        Instantiate(damageIndicator, transform.position, Quaternion.identity).GetComponent<DamageIndicator>().SetIndicator(damage, spr.sprite.bounds.extents);
    }

    public virtual void Die()
    {
        if (corpse) 
        {
            SpriteRenderer corpseSprite = Instantiate(corpse, transform.position, Quaternion.identity).GetComponent<Corpse>().sr;
            corpseSprite.flipY = spr.flipX;
            corpseSprite.sprite = spr.sprite;
        }
        Destroy(gameObject);
    }
    
    void Unwhite()
    {
        spr.material = normal;
    }

    protected virtual void Update()
    {
        spr.transform.localScale = Vector3.Lerp(spr.transform.localScale, Vector3.one, squashSpeed * Time.deltaTime);
        if (hc.health <= 0f)
        {
            Die();
        }
    }
}