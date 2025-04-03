using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;
    private static GameObject inventoryObject;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void InitializeStaticFields()
    {
        if (instance == null)
        {
            inventoryObject = new GameObject("InventoryManager");
            instance = inventoryObject.AddComponent<Inventory>();
            DontDestroyOnLoad(inventoryObject);
        }
    }

    public List<InventorySlot> inventorySlots = new List<InventorySlot>();
    public int maxSlots = 30;

    public delegate void OnInventoryChanged();
    public OnInventoryChanged onInventoryChangedCallback;

    void Start()
    {
        onInventoryChangedCallback?.Invoke();
    }

    public bool AddItem(Item item, int quantity)
    {
        if (item.isStackable)
        {
            foreach (InventorySlot slot in inventorySlots)
            {
                if (slot.item == item && slot.quantity < item.maxStack)
                {
                    slot.quantity = Mathf.Min(slot.quantity + quantity, item.maxStack);
                    onInventoryChangedCallback?.Invoke();
                    Debug.Log($"Stacked {quantity} of {item.itemName} in existing slot");
                    return true;
                }
            }
        }

        if (inventorySlots.Count < maxSlots)
        {
            inventorySlots.Add(new InventorySlot(item, quantity));
            onInventoryChangedCallback?.Invoke();
            Debug.Log($"Added new slot with {quantity} of {item.itemName}");
            return true;
        }

        Debug.LogWarning("Inventory full, cannot add " + item.itemName);
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
                {
                    inventorySlots.RemoveAt(i);
                }

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
