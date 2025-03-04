using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Item item;
    public int quantity = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            bool added = Inventory.instance.AddItem(item, quantity);
            if (added)
            {
                Debug.Log($"Picked up {item.itemName} x{quantity}");
                Destroy(gameObject);
            }
        }
    }
}
