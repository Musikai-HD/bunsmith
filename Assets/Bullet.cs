using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Rigidbody2D rb;

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
}
