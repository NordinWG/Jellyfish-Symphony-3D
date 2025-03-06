using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Item item;
    public int quantity;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            bool added = Inventory.instance.AddItem(item, quantity);
            
            if (added)
            {
                Destroy(gameObject);
            }
        }
    }
}