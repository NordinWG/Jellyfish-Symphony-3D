using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance; // Singleton pattern for easy access

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public List<InventorySlot> inventorySlots = new List<InventorySlot>();
    public int maxSlots = 30; // Set a limit to inventory size

    public delegate void OnInventoryChanged();
    public OnInventoryChanged onInventoryChangedCallback;

    void Start()
    {
        if (onInventoryChangedCallback != null)
            onInventoryChangedCallback.Invoke();

        // Debugging: Add a test item at the start
        Item testItem = Resources.Load<Item>("HealthPotion"); // Make sure you have a ScriptableObject named "HealthPotion"
        if (testItem != null)
        {
            AddItem(testItem, 1);
        }
    }

    public bool AddItem(Item item, int quantity)
    {
        Debug.Log("Attempting to add item: " + item.itemName); // Debug log

        // Check if the item is stackable and already exists
        if (item.isStackable)
        {
            foreach (InventorySlot slot in inventorySlots)
            {
                if (slot.item == item && slot.quantity < item.maxStack)
                {
                    slot.quantity = Mathf.Min(slot.quantity + quantity, item.maxStack);
                    Debug.Log($"Stacking {item.itemName}, new quantity: {slot.quantity}");
                    onInventoryChangedCallback?.Invoke();
                    return true;
                }
            }
        }

        // If it's not stackable or no stack exists, find an empty slot
        if (inventorySlots.Count < maxSlots)
        {
            inventorySlots.Add(new InventorySlot(item, quantity));
            Debug.Log($"Added new item: {item.itemName}, Quantity: {quantity}");
            onInventoryChangedCallback?.Invoke();
            return true;
        }

        Debug.Log("Inventory is full!");
        return false;
    }

    public void RemoveItem(Item item, int quantity)
    {
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            if (inventorySlots[i].item == item)
            {
                inventorySlots[i].quantity -= quantity;
                if (inventorySlots[i].quantity <= 0)
                    inventorySlots.RemoveAt(i);

                Debug.Log($"Removed {item.itemName}, Remaining: {inventorySlots[i].quantity}");
                onInventoryChangedCallback?.Invoke();
                return;
            }
        }
    }
}

[System.Serializable]
public class InventorySlot
{
    public Item item;
    public int quantity;

    public InventorySlot(Item newItem, int newQuantity)
    {
        item = newItem;
        quantity = newQuantity;
    }
}
