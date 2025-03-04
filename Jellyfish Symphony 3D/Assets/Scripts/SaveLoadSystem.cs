using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveLoadSystem : MonoBehaviour
{
    public enum InventoryItem { None, Drownie, Shell, Staff }
    public List<InventoryItem> inventory = new List<InventoryItem>();

    private string[] saveSlotNames = { "SaveSlot1", "SaveSlot2", "SaveSlot3" };

    public GameObject saveLoadPanel; // Assign in Inspector (UI Panel for slots)
    public Text[] saveSlotTexts; // Assign 3 Text UI elements (slot info)

    void Start()
    {
        UpdateSaveSlotUI(); // Update UI on start
    }

    public void SaveGame(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= saveSlotNames.Length) return;

        PlayerPrefs.SetFloat(saveSlotNames[slotIndex] + "_PosX", transform.position.x);
        PlayerPrefs.SetFloat(saveSlotNames[slotIndex] + "_PosY", transform.position.y);
        PlayerPrefs.SetFloat(saveSlotNames[slotIndex] + "_PosZ", transform.position.z);

        PlayerPrefs.SetInt(saveSlotNames[slotIndex] + "_InventorySize", inventory.Count);
        for (int i = 0; i < inventory.Count; i++)
        {
            PlayerPrefs.SetInt(saveSlotNames[slotIndex] + "_Item_" + i, (int)inventory[i]);
        }

        PlayerPrefs.Save();
        UpdateSaveSlotUI();
        Debug.Log("Game saved to " + saveSlotNames[slotIndex]);
    }

    public void LoadGame(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= saveSlotNames.Length) return;

        float posX = PlayerPrefs.GetFloat(saveSlotNames[slotIndex] + "_PosX", 0f);
        float posY = PlayerPrefs.GetFloat(saveSlotNames[slotIndex] + "_PosY", 0f);
        float posZ = PlayerPrefs.GetFloat(saveSlotNames[slotIndex] + "_PosZ", 0f);
        transform.position = new Vector3(posX, posY, posZ);

        int inventorySize = PlayerPrefs.GetInt(saveSlotNames[slotIndex] + "_InventorySize", 0);
        inventory.Clear();
        for (int i = 0; i < inventorySize; i++)
        {
            int itemValue = PlayerPrefs.GetInt(saveSlotNames[slotIndex] + "_Item_" + i, 0);
            inventory.Add((InventoryItem)itemValue);
        }

        Debug.Log("Game loaded from " + saveSlotNames[slotIndex]);
    }

    public void ClearSaveSlot(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= saveSlotNames.Length) return;

        PlayerPrefs.DeleteKey(saveSlotNames[slotIndex] + "_PosX");
        PlayerPrefs.DeleteKey(saveSlotNames[slotIndex] + "_PosY");
        PlayerPrefs.DeleteKey(saveSlotNames[slotIndex] + "_PosZ");

        int inventorySize = PlayerPrefs.GetInt(saveSlotNames[slotIndex] + "_InventorySize", 0);
        for (int i = 0; i < inventorySize; i++)
        {
            PlayerPrefs.DeleteKey(saveSlotNames[slotIndex] + "_Item_" + i);
        }
        PlayerPrefs.DeleteKey(saveSlotNames[slotIndex] + "_InventorySize");

        PlayerPrefs.Save();
        UpdateSaveSlotUI();
        Debug.Log("Save slot " + saveSlotNames[slotIndex] + " cleared.");
    }

    public void OpenSaveLoadUI()
    {
        saveLoadPanel.SetActive(true);
        UpdateSaveSlotUI();
    }

    public void CloseSaveLoadUI()
    {
        saveLoadPanel.SetActive(false);
    }

    void UpdateSaveSlotUI()
    {
        for (int i = 0; i < saveSlotNames.Length; i++)
        {
            int inventorySize = PlayerPrefs.GetInt(saveSlotNames[i] + "_InventorySize", 0);
            if (inventorySize > 0)
            {
                saveSlotTexts[i].text = "Slot " + (i + 1) + " (Saved)";
            }
            else
            {
                saveSlotTexts[i].text = "Slot " + (i + 1) + " (Empty)";
            }
        }
    }
}
