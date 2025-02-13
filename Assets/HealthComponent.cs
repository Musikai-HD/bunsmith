using UnityEngine;

[System.Serializable]
public class HealthComponent
{
    public float maxHealth;
    public float health;

    public float maxShield;
    public float shield;

    public bool invincible;

    // Update is called once per frame
    void Update()
    {
        if (health > maxHealth)
        {
            health = maxHealth;
        }
    }

    public void Damage(float damage)
    {
        if (!invincible)
        {
            health -= damage;
        }
    }

}
