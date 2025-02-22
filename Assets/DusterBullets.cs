using UnityEngine;

[CreateAssetMenu(fileName = "Duster", menuName = "Weapon/Duster Bullets", order = 0)]
public class DusterBullets : WeaponBullets
{
    public float lifetimeCap;
    public override void ExtraThings(Weapon wep)
    {
        wep.BulletLifetime = Mathf.Min(wep.BulletLifetime, lifetimeCap);
    }
}
