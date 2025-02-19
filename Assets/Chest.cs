using UnityEngine;

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
        groundItem.Initialize();
        Destroy(gameObject);
    }
}
