using UnityEngine;
using UnityEngine.UIElements;

public class Bullet : MonoBehaviour
{
    public Rigidbody2D rb;
    public ParticleSystem mainParticles, hitParticles;
    public SpriteRenderer sr;
    public Hitbox hb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        Invoke("Die", 3f);
    }

    void Die()
    {
        Destroy(gameObject);
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (hb.Hit(col))
        {
            hitParticles.Play();
            mainParticles.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            sr.enabled = false;
            CancelInvoke("Die");
            Invoke("Die", 1f);
        }
    }
}
