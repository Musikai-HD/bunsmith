using UnityEngine;

public class ItemWrapper : ScriptableObject
{
    public string name;
    public Sprite menuSprite;
    public Rarity rarity;

    public enum Rarity
    {
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary
    }
}
