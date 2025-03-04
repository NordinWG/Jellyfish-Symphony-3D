using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // TextMeshPro for UI text

public class InventoryUI : MonoBehaviour
{
    public GameObject inventoryPanel;
    public Transform slotContainer;
    public GameObject slotPrefab;

    private List<GameObject> slots = new List<GameObject>();

    private void Start()
    {
        Inventory.instance.onInventoryChangedCallback += UpdateUI;
        UpdateUI();
    }

    public void ToggleInventory()
    {
        inventoryPanel.SetActive(!inventoryPanel.activeSelf);
        Debug.Log("Toggling inventory UI. Active: " + inventoryPanel.activeSelf);
    }

    void UpdateUI()
    {
        Debug.Log("Updating Inventory UI...");

        // Clear old UI
        foreach (GameObject slot in slots)
            Destroy(slot);

        slots.Clear();

        // Populate new UI
        foreach (InventorySlot slot in Inventory.instance.inventorySlots)
        {
            Debug.Log("Adding Item to UI: " + slot.item.itemName); // Debugging

            GameObject newSlot = Instantiate(slotPrefab, slotContainer);
            newSlot.transform.GetChild(0).GetComponent<Image>().sprite = slot.item.itemIcon;
            newSlot.transform.GetChild(1).GetComponent<TMP_Text>().text = slot.quantity > 1 ? slot.quantity.ToString() : "";

            slots.Add(newSlot);
        }
    }
}
