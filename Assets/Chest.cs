using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Chest : Interactable
{
    public ItemPool pool;
    [SerializeField] GameObject groundItemPrefab;
    public override void Interact()
    {
        ItemWrapper item = ItemPicker.PickItem(Instantiate(pool));
        Debug.Log(item);
        GroundItem groundItem = Instantiate(groundItemPrefab, transform.position, Quaternion.identity).GetComponent<GroundItem>();
        groundItem.part = item;
        switch (groundItem.part.rarity)
        {
            case ItemWrapper.Rarity.Common:
                GameManager.instance.score += 100;
                break;
            case ItemWrapper.Rarity.Uncommon:
                GameManager.instance.score += 200;
                break;
            case ItemWrapper.Rarity.Rare:
                GameManager.instance.score += 350;
                break;
            case ItemWrapper.Rarity.Epic:
                GameManager.instance.score += 500;
                break;
            case ItemWrapper.Rarity.Legendary:
                GameManager.instance.score += 1000;
                break;
        }
        groundItem.Initialize();
        Destroy(gameObject);
    }
}
