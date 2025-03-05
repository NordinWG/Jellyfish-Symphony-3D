using System.Collections.Generic;
using UnityEngine;
using TMPro; // Import for TextMeshPro
using UnityEngine.UI; // Import for UI elements

public class InventoryUI : MonoBehaviour
{
    public GameObject inventoryPanel;
    public Transform slotContainer;
    public GameObject slotPrefab;
    public TextMeshProUGUI itemDescriptionText; // Reference to description text
    public Image itemDisplayIcon; // Reference to the UI image for displaying the selected item icon

    private List<GameObject> slots = new List<GameObject>();

    private void Start()
    {
        Inventory.instance.onInventoryChangedCallback += UpdateUI;
        UpdateUI();
    }

    public void ToggleInventory()
    {
        inventoryPanel.SetActive(!inventoryPanel.activeSelf);

        // Auto-select the first item when opening inventory
        if (inventoryPanel.activeSelf && Inventory.instance.inventorySlots.Count > 0)
        {
            SelectItem(0); // Automatically select the first slot
        }
    }

    void UpdateUI()
    {
        // Clear old UI slots
        foreach (GameObject slot in slots)
            Destroy(slot);

        slots.Clear();

        // Populate new UI with updated slots
        for (int i = 0; i < Inventory.instance.inventorySlots.Count; i++)
        {
            InventorySlot slot = Inventory.instance.inventorySlots[i];
            GameObject newSlot = Instantiate(slotPrefab, slotContainer);

            // Set item icon
            Image itemIcon = newSlot.transform.GetChild(0).GetComponent<Image>();
            itemIcon.sprite = slot.item.itemIcon;

            // Correctly position the item icon inside the slot
            RectTransform iconRect = itemIcon.GetComponent<RectTransform>();
            iconRect.anchorMin = new Vector2(0, 0);
            iconRect.anchorMax = new Vector2(1, 1);
            iconRect.offsetMin = new Vector2(0, 0); // Left & Bottom
            iconRect.offsetMax = new Vector2(0, 0);   // Right & Top

            // Set item quantity text
            TextMeshProUGUI quantityText = newSlot.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            if (slot.quantity > 1)
            {
                quantityText.text = slot.quantity.ToString();

                // Position the quantity at the top-right corner of the slot
                RectTransform quantityRect = quantityText.GetComponent<RectTransform>();
                quantityRect.anchorMin = new Vector2(1, 1);
                quantityRect.anchorMax = new Vector2(1, 1);
                quantityRect.pivot = new Vector2(1, 1);
                quantityRect.anchoredPosition = new Vector2(5, -10);

                // Adjust width for better visibility
                quantityRect.sizeDelta = new Vector2(40, quantityRect.sizeDelta.y);
            }
            else
            {
                quantityText.text = ""; // No quantity displayed if it's 1
            }

            // Add a click event to select this slot
            int index = i;
            Button slotButton = newSlot.GetComponent<Button>();
            slotButton.onClick.AddListener(() => SelectItem(index));

            slots.Add(newSlot);
        }

        // Auto-select first item when inventory updates
        if (slots.Count > 0)
        {
            SelectItem(0);
        }
    }

    // Handle item selection from the UI
    void SelectItem(int index)
    {
        if (index < 0 || index >= Inventory.instance.inventorySlots.Count) return;

        InventorySlot selectedSlot = Inventory.instance.inventorySlots[index];
        DisplayItemDetails(selectedSlot.item);
    }

    // Display the item icon and description in the designated panel
    void DisplayItemDetails(Item item)
    {
        if (item != null)
        {
            itemDescriptionText.text = $"<b>{item.itemName}</b>\n{item.itemDescription}";
            itemDisplayIcon.sprite = item.itemIcon;
            itemDisplayIcon.color = Color.white; // Ensure icon is visible (in case it was hidden)
        }
    }
}
