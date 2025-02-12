using UnityEngine;

public class ItemWrapper : ScriptableObject
{
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
}
