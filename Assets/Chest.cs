using UnityEngine;

public class Chest : Interactable
{
    public ItemPool pool;
    public override void Interact()
    {
        ItemWrapper item = ItemPicker.PickItem(Instantiate(pool));
        Debug.Log(item);
        if (item is WeaponComponent) GameManager.instance.gw.Equip(item as WeaponComponent);
        Destroy(gameObject);
    }
}
