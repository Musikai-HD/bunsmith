using UnityEngine;

public class WeaponPart : WeaponComponent
{
    public float damageMult = 1f;
    public float fireRateMult = 1f;
    public float accuracyMult = 1f;
    public float magMult = 1f;
    public float reloadMult = 1f;
    public float bulletSpeedMult = 1f;
    public float bulletCountMult = 1f;

    public override string GetDescription()
    {
        return
        GetPercent("Damage", damageMult) +
        GetPercent("Fire Rate", fireRateMult) +
        GetPercent("Accuracy", accuracyMult) +
        GetPercent("Mag Size", magMult) +
        GetPercent("Reload Speed", reloadMult) +
        GetPercent("Bullet Speed", bulletSpeedMult) +
        GetPercent("Projectiles", bulletCountMult);
    }

    string GetPercent(string _posttext, float _val)
    {
        // Calculate the percentage change relative to base 1
        float percentageChange = (_val - 1f) * 100f;

        // Format the result as a string with a "+" or "-" sign
        string result = percentageChange >= 0f
            ? $"+{percentageChange:0.##}%"
            : $"-{-percentageChange:0.##}%";

        if (_val == 1f) return "";

        return result + " " + _posttext +"\n";
    }

    public virtual void ExtraThings(Weapon wep) {}
}