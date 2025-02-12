using UnityEngine;

[CreateAssetMenu(fileName = "New Bullet Type", menuName = "Weapon/Bullets", order = 4)]
public class WeaponBullets : WeaponPart
{
    public BulletStatus status;
    public BulletType type;
    public enum BulletStatus
    {
        Normal,
        Poison,
        Frost
    }
    public enum BulletType
    {
        Bullet,
        Rocket,
        Particle
    }
}
