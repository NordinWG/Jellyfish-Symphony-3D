using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public GameObject inventoryPanel;
    public Transform slotContainer;
    public GameObject slotPrefab;
    public TextMeshProUGUI itemDescriptionText;
    public Image itemDisplayIcon;

    private List<GameObject> slots = new List<GameObject>();

    private void Start()
    {
        Inventory.instance.onInventoryChangedCallback += UpdateUI;
        UpdateUI();
    }

    public void ToggleInventory()
    {
        inventoryPanel.SetActive(!inventoryPanel.activeSelf);

        if (inventoryPanel.activeSelf && Inventory.instance.inventorySlots.Count > 0)
        {
            SelectItem(0);
        }
    }

    void UpdateUI()
    {
        foreach (GameObject slot in slots)
            Destroy(slot);

        slots.Clear();

        for (int i = 0; i < Inventory.instance.inventorySlots.Count; i++)
        {
            InventorySlot slot = Inventory.instance.inventorySlots[i];
            GameObject newSlot = Instantiate(slotPrefab, slotContainer);

            Image itemIcon = newSlot.transform.GetChild(0).GetComponent<Image>();
            itemIcon.sprite = slot.item.itemIcon;

            RectTransform iconRect = itemIcon.GetComponent<RectTransform>();
            iconRect.anchorMin = new Vector2(0, 0);
            iconRect.anchorMax = new Vector2(1, 1);
            iconRect.offsetMin = new Vector2(0, 0);
            iconRect.offsetMax = new Vector2(0, 0);

            TextMeshProUGUI quantityText = newSlot.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            if (slot.quantity > 1)
            {
                quantityText.text = slot.quantity.ToString();

                RectTransform quantityRect = quantityText.GetComponent<RectTransform>();
                quantityRect.anchorMin = new Vector2(1, 1);
                quantityRect.anchorMax = new Vector2(1, 1);
                quantityRect.pivot = new Vector2(1, 1);
                quantityRect.anchoredPosition = new Vector2(5, -10);

                quantityRect.sizeDelta = new Vector2(40, quantityRect.sizeDelta.y);
            }
            else
            {
                quantityText.text = "";
            }

            int index = i;
            Button slotButton = newSlot.GetComponent<Button>();
            slotButton.onClick.AddListener(() => SelectItem(index));

            slots.Add(newSlot);
        }

        if (slots.Count > 0)
        {
            SelectItem(0);
        }
    }

    void SelectItem(int index)
    {
        if (index < 0 || index >= Inventory.instance.inventorySlots.Count) return;

        InventorySlot selectedSlot = Inventory.instance.inventorySlots[index];
        DisplayItemDetails(selectedSlot.item);
    }

    void DisplayItemDetails(Item item)
    {
        if (item != null)
        {
            itemDescriptionText.text = $"<b>{item.itemName}</b>\n{item.itemDescription}";
            itemDisplayIcon.sprite = item.itemIcon;
            itemDisplayIcon.color = Color.white;
        }
    }
}
