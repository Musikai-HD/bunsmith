using JetBrains.Annotations;
using TMPro;
using UnityEngine;

public class GroundItem : Interactable
{
    public ItemWrapper part;
    [SerializeField] TextMeshPro nameText, descText;
    public Color commonColor, uncommonColor, rareColor, epicColor, legendaryColor;
    public AudioClip pickupSound;

    public void Initialize()
    {
        nameText.text = $"{part.name}\n<size=70%>{part.rarity.ToString()} {ItemWrapper.TypeToName(part)}</size>";
        descText.text = part.GetDescription();
        nameText.color = GetRarityColor(part.rarity);
    }

    public override void Interact()
    {
        base.Interact();
        Debug.Log(part.GetDescription());
        GameManager.instance.pc.weapon.Equip(part as WeaponComponent);
        AudioManager.instance.Play(pickupSound);
        Destroy(gameObject);
    }

    Color GetRarityColor(ItemWrapper.Rarity rarity)
    {
        switch (rarity)
        {
            case ItemWrapper.Rarity.Common:
                return commonColor;
            case ItemWrapper.Rarity.Uncommon:
                return uncommonColor;
            case ItemWrapper.Rarity.Rare:
                return rareColor;
            case ItemWrapper.Rarity.Epic:
                return epicColor;
            case ItemWrapper.Rarity.Legendary:
                return legendaryColor;
            default:
                return Color.white;
        }
    }
}
