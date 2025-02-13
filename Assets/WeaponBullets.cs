using UnityEngine;

[CreateAssetMenu(fileName = "New Bullet Type", menuName = "Weapon/Bullets", order = 4)]
public class WeaponBullets : WeaponPart
{
    public BulletType type;
    public Status.StatusType status;
    public float statusTime, statusDamage;
    public enum BulletType
    {
        Bullet,
        Rocket,
        Particle
    }
}
