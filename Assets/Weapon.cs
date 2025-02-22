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

    public float Damage;
    public float FireRate;
    public float Accuracy;
    public int Mag;
    public float Reload;
    public float BulletSpeed;
    public int BulletCount;
    public Status.StatusType BulletStatus;
    public float StatusTime;
    public float StatusDamage;
    public int PierceCount;
    public float BulletLifetime;

    public void InitializeWeapon()
    {
        BaseSprite = frame.sprite;
        StockSprite = stock == null ? null : stock.sprite;
        BarrelSprite = barrel == null ? null : barrel.sprite;

        Damage = Mathf.Round(10f * frame.damage * 
        (stock == null ? 1f : stock.damageMult) * 
        (barrel == null ? 1f : barrel.damageMult) * 
        (attachment == null ? 1f : attachment.damageMult) * 
        (bullets == null ? 1f : bullets.damageMult)) / 10f;

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

        Mag = Mathf.CeilToInt(frame.mag * 
        (stock == null ? 1f : stock.magMult) * 
        (barrel == null ? 1f : barrel.magMult) * 
        (attachment == null ? 1f : attachment.magMult) * 
        (bullets == null ? 1f : bullets.magMult));

        BulletCount = (int)(frame.bulletCount * 
        (stock == null ? 1f : stock.bulletCountMult) * 
        (barrel == null ? 1f : barrel.bulletCountMult) * 
        (attachment == null ? 1f : attachment.bulletCountMult) * 
        (bullets == null ? 1f : bullets.bulletCountMult));

        BulletStatus = bullets == null ? Status.StatusType.None : bullets.status;

        PierceCount = bullets == null ? 0 : bullets.pierceCount;

        BulletLifetime = frame == null ? 0f : frame.bulletLiftetime;

        stock?.ExtraThings(this);
        barrel?.ExtraThings(this);
        attachment?.ExtraThings(this);
        bullets?.ExtraThings(this);
    }

    public void CopyFrom(Weapon other)
    {
        frame = other.frame;
        stock = other.stock;
        barrel = other.barrel;
        attachment = other.attachment;
        bullets = other.bullets;
    }

}
