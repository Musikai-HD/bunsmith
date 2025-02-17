using UnityEngine;

public class Corpse : MonoBehaviour
{
    bool vanishing;
    [SerializeField] SpriteRenderer sr;
    void Start()
    {
        Invoke("Die", 4f);
    }

    void Update()
    {
        if (vanishing)
        {
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, sr.color.a - Time.deltaTime);
            if (sr.color.a <= 0f)
            {
                Destroy(gameObject);
            }
        }
    }

    void Die()
    {
        vanishing = true;
    }
}
