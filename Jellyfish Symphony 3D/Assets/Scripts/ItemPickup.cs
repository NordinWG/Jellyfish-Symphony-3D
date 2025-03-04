using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Item item;
    public int quantity;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log($"ItemPickup: Player collided with {gameObject.name}. Attempting pickup...");
            bool added = Inventory.instance.AddItem(item, quantity);
            
            if (added)
            {
                Debug.Log($"ItemPickup: Successfully picked up {quantity}x {item.itemName}. Destroying object.");
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("ItemPickup: Inventory full! Could not pick up item.");
            }
        }
    }
}
