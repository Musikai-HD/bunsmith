using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Bullet : MonoBehaviour
{
    public Rigidbody2D rb;
    public ParticleSystem mainParticles, hitParticles;
    public SpriteRenderer sr;
    public Hitbox hb;
    public float lifetime;
    public int pierceCount;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        Invoke("Die", lifetime);
    }

    void Die()
    {
        Destroy(gameObject);
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (hb.Hit(col) == 1)
        {
            if (pierceCount > 0)
            {
                pierceCount--;
            }
            else
            {
                Explode();
            }
        }
        if (hb.Hit(col) == 0) 
        {
            Explode();
        }
    }

    void Explode()
    {
        hitParticles.Play();
        mainParticles.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        CancelInvoke("Die");
        sr.enabled = false;
        rb.linearVelocity = Vector2.zero;
        hb.canHit = false;
        Invoke("Die", 1f);
    }
}
