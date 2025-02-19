using UnityEngine;

public class ItemWrapper : ScriptableObject
{
    public string description;
    public Sprite menuSprite;
    public Rarity rarity;

    public override string ToString() => name;

    public enum Rarity
    {
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary
    }

    public enum ItemType
    {
        Item,
        WeaponFrame,
        WeaponStock,
        WeaponBarrel,
        WeaponAttachment,
        WeaponBullets,
        NONE
    }

    public virtual string GetDescription()
    {
        return description;
    }

    public static string TypeToName(ItemWrapper type)
    {
        switch (type)
        {
            case WeaponFrame:
                return "Frame";
            case WeaponStock:
                return "Stock";
            case WeaponBarrel:
                return "Barrel";
            case WeaponAttachment:
                return "Attachment";
            case WeaponBullets:
                return "Ammo";
            default:
                return "?part?";
        }
    }
}
