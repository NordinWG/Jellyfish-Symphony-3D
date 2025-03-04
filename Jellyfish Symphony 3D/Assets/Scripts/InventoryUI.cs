using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    public GameObject inventoryPanel;
    public Transform slotContainer;
    public GameObject slotPrefab;

    private List<GameObject> slots = new List<GameObject>();

    private void Start()
    {
        Debug.Log("InventoryUI: Start() called. Subscribing to inventory changes.");
        Inventory.instance.onInventoryChangedCallback += UpdateUI;
        UpdateUI();
    }

    public void ToggleInventory()
    {
        bool isActive = !inventoryPanel.activeSelf;
        inventoryPanel.SetActive(isActive);
        Debug.Log($"InventoryUI: Toggling inventory. Now active: {isActive}");
    }

    void UpdateUI()
    {
        Debug.Log("InventoryUI: Updating UI...");

        foreach (GameObject slot in slots)
        {
            Debug.Log("InventoryUI: Destroying old slot.");
            Destroy(slot);
        }

        slots.Clear();
        Debug.Log("InventoryUI: Cleared old inventory slots.");

        foreach (InventorySlot slot in Inventory.instance.inventorySlots)
        {
            Debug.Log($"InventoryUI: Creating new slot for {slot.item.itemName} (x{slot.quantity})");
            GameObject newSlot = Instantiate(slotPrefab, slotContainer);
            newSlot.transform.GetChild(0).GetComponent<Image>().sprite = slot.item.itemIcon;
            newSlot.transform.GetChild(1).GetComponent<TMP_Text>().text = slot.quantity > 1 ? slot.quantity.ToString() : "";
            slots.Add(newSlot);
        }

        Debug.Log("InventoryUI: Finished updating UI.");
    }
}
