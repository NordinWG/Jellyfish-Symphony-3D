using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    private void Awake()
    {
        Debug.Log("Inventory: Awake() called.");

        if (instance == null)
        {
            instance = this;
            Debug.Log("Inventory: Instance set.");
        }
        else
        {
            Debug.Log("Inventory: Duplicate instance found! Destroying this instance.");
            Destroy(gameObject);
        }
    }

    public List<InventorySlot> inventorySlots = new List<InventorySlot>();
    public int maxSlots = 30; // Set a limit to inventory size

    public delegate void OnInventoryChanged();
    public OnInventoryChanged onInventoryChangedCallback;

    void Start()
    {
        Debug.Log("Inventory: Start() called.");
        onInventoryChangedCallback?.Invoke();
    }

    public bool AddItem(Item item, int quantity)
    {
        Debug.Log($"Inventory: Trying to add {quantity}x {item.itemName}");

        if (item.isStackable)
        {
            Debug.Log("Inventory: Item is stackable, checking existing stacks.");

            foreach (InventorySlot slot in inventorySlots)
            {
                if (slot.item == item && slot.quantity < item.maxStack)
                {
                    slot.quantity = Mathf.Min(slot.quantity + quantity, item.maxStack);
                    Debug.Log($"Inventory: Stacked {item.itemName}. New quantity: {slot.quantity}");
                    onInventoryChangedCallback?.Invoke();
                    return true;
                }
            }
        }

        if (inventorySlots.Count < maxSlots)
        {
            Debug.Log("Inventory: No existing stack found, adding a new slot.");
            inventorySlots.Add(new InventorySlot(item, quantity));
            onInventoryChangedCallback?.Invoke();
            return true;
        }

        Debug.Log("Inventory: Inventory is full! Cannot add item.");
        return false;
    }

    public void RemoveItem(Item item, int quantity)
    {
        Debug.Log($"Inventory: Trying to remove {quantity}x {item.itemName}");

        for (int i = 0; i < inventorySlots.Count; i++)
        {
            if (inventorySlots[i].item == item)
            {
                inventorySlots[i].quantity -= quantity;
                Debug.Log($"Inventory: Removed {quantity}x {item.itemName}. Remaining: {inventorySlots[i].quantity}");

                if (inventorySlots[i].quantity <= 0)
                {
                    Debug.Log($"Inventory: {item.itemName} quantity is 0. Removing from inventory.");
                    inventorySlots.RemoveAt(i);
                }

                onInventoryChangedCallback?.Invoke();
                return;
            }
        }
        Debug.Log("Inventory: Item not found in inventory.");
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
        Debug.Log($"InventorySlot: Created new slot for {item.itemName} with {quantity}x quantity.");
    }
}