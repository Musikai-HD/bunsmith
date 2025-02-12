using UnityEngine;

[System.Serializable]
public class Weapon
{
    public WeaponFrame frame;
    public WeaponStock stock;
    public WeaponBarrel barrel;
    public WeaponAttachment attachment;
    public WeaponBullets bullets;

    public Sprite StockSprite {get; private set;}
    public Sprite BaseSprite {get; private set;}
    public Sprite BarrelSprite {get; private set;}
    public float Damage {get; private set;}
    public float FireRate {get; private set;}
    public float Accuracy {get; private set;}
    public int Mag {get; private set;}
    public float Reload {get; private set;}
    public float BulletSpeed {get; private set;}
    public int BulletCount {get; private set;}

    void Start()
    {
        //InitializeWeapon();
    }

    public void InitializeWeapon()
    {
        BaseSprite = frame.sprite;
        StockSprite = stock == null ? null : stock.sprite;
        BarrelSprite = barrel == null ? null : barrel.sprite;

        Damage = frame.damage * 
        (stock == null ? 1f : stock.damageMult) * 
        (barrel == null ? 1f : barrel.damageMult) * 
        (attachment == null ? 1f : attachment.damageMult) * 
        (bullets == null ? 1f : bullets.damageMult);

        FireRate = frame.fireRate / 
        ((stock == null ? 1f : stock.fireRateMult) * 
        (barrel == null ? 1f : barrel.fireRateMult) * 
        (attachment == null ? 1f : attachment.fireRateMult) * 
        (bullets == null ? 1f : bullets.fireRateMult));

        Reload = frame.reload / 
        ((stock == null ? 1f : stock.reloadMult) * 
        (barrel == null ? 1f : barrel.reloadMult) * 
        (attachment == null ? 1f : attachment.reloadMult) * 
        (bullets == null ? 1f : bullets.reloadMult));

        Accuracy = frame.accuracy / 
        ((stock == null ? 1f : stock.accuracyMult) * 
        (barrel == null ? 1f : barrel.accuracyMult) * 
        (attachment == null ? 1f : attachment.accuracyMult) * 
        (bullets == null ? 1f : bullets.accuracyMult));

        BulletSpeed = frame.bulletSpeed * 
        (stock == null ? 1f : stock.bulletSpeedMult) * 
        (barrel == null ? 1f : barrel.bulletSpeedMult) * 
        (attachment == null ? 1f : attachment.bulletSpeedMult) * 
        (bullets == null ? 1f : bullets.bulletSpeedMult);

        Mag = (int)(frame.mag * 
        (stock == null ? 1f : stock.magMult) * 
        (barrel == null ? 1f : barrel.magMult) * 
        (attachment == null ? 1f : attachment.magMult) * 
        (bullets == null ? 1f : bullets.magMult));

        BulletCount = (int)(frame.bulletCount * 
        (stock == null ? 1f : stock.bulletCountMult) * 
        (barrel == null ? 1f : barrel.bulletCountMult) * 
        (attachment == null ? 1f : attachment.bulletCountMult) * 
        (bullets == null ? 1f : bullets.bulletCountMult));
    }

}
