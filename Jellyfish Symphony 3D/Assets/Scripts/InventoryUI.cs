using System.Collections.Generic;
using UnityEngine;
using TMPro; // Make sure you have this import for TextMeshPro
using UnityEngine.UI;  // Import UnityEngine.UI for Image

public class InventoryUI : MonoBehaviour
{
    public GameObject inventoryPanel;
    public Transform slotContainer;
    public GameObject slotPrefab;
    public TextMeshProUGUI itemDescriptionText; // Reference to the description text on the left side of the screen

    private List<GameObject> slots = new List<GameObject>();

    private void Start()
    {
        Inventory.instance.onInventoryChangedCallback += UpdateUI;
        UpdateUI();
    }

    public void ToggleInventory()
    {
        inventoryPanel.SetActive(!inventoryPanel.activeSelf);
    }

    void UpdateUI()
    {
        // Clear old UI
        foreach (GameObject slot in slots)
            Destroy(slot);

        slots.Clear();

        // Populate new UI
        foreach (InventorySlot slot in Inventory.instance.inventorySlots)
        {
            GameObject newSlot = Instantiate(slotPrefab, slotContainer);

            // Set item icon
            newSlot.transform.GetChild(0).GetComponent<Image>().sprite = slot.item.itemIcon;

            // Set item quantity and position it
            TextMeshProUGUI quantityText = newSlot.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            if (slot.quantity > 1)
            {
                quantityText.text = slot.quantity.ToString();
                // Position the quantity at the top-right corner of the slot
                RectTransform quantityRect = quantityText.GetComponent<RectTransform>();
                quantityRect.anchorMin = new Vector2(1, 1); // Anchor to the top-right
                quantityRect.anchorMax = new Vector2(1, 1); // Anchor to the top-right
                quantityRect.pivot = new Vector2(1, 1); // Pivot to the top-right
                quantityRect.anchoredPosition = new Vector2(-15, -15); // Move it more down and left
                
                // Adjust the width of the TextMeshPro box (make it wider)
                quantityRect.sizeDelta = new Vector2(40, quantityRect.sizeDelta.y); // Increase width, keep height same
            }
            else
            {
                quantityText.text = ""; // No quantity displayed if it's 1
            }

            // Add a click event to select this slot
            int index = slots.Count;
            Button slotButton = newSlot.GetComponent<Button>();
            slotButton.onClick.AddListener(() => SelectItem(index));

            slots.Add(newSlot);
        }
    }

    // Handle item selection from the UI
    void SelectItem(int index)
    {
        if (index < 0 || index >= Inventory.instance.inventorySlots.Count) return;

        InventorySlot selectedSlot = Inventory.instance.inventorySlots[index];
        DisplayItemDescription(selectedSlot.item);
    }

    // Display the item description on the left side of the screen
    void DisplayItemDescription(Item item)
    {
        if (item != null)
        {
            itemDescriptionText.text = $"<b>{item.itemName}</b>\n{item.itemDescription}";
        }
    }
}