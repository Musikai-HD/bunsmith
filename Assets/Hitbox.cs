using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public LayerMask hittableLayers;
    public float damage;

    void OnTriggerEnter2D(Collider2D col)
    {
        if ((hittableLayers & 1 << col.gameObject.layer) == 1 << col.gameObject.layer)
        {
            col.gameObject.GetComponent<Damageable>().Damage(damage);
            Destroy(gameObject);
        }
    }
}
