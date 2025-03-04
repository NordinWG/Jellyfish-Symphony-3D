using System.Collections.Generic;
using UnityEngine;

public class SaveLoadSystem : MonoBehaviour

{
   {
    // Enum to define the possible inventory items
    public enum InventoryItem
    {
        None,
        Drownie,
        Shell,
        Staff
    }

    // Inventory system (a list of items stored as enum values)
    public List<InventoryItem> inventory = new List<InventoryItem>();

    // Names for the save slots
    private string[] saveSlotNames = { "SaveSlot1", "SaveSlot2", "SaveSlot3" };

    // Interval in seconds between auto-saves
    public float autoSaveInterval = 30f;

    private float saveTimer;

    void Start()
    {
        // Start auto-save timer
        saveTimer = autoSaveInterval;
    }

    void Update()
    {
        // Update the save timer every frame
        saveTimer -= Time.deltaTime;

        // Check if it's time to auto-save
        if (saveTimer <= 0f)
        {
            // Automatically save to the first save slot for example (can be rotated to other slots)
            AutoSave(0);

            // Reset the timer
            saveTimer = autoSaveInterval;
        }
    }

    // Auto-save function that saves to a specific slot (you can add logic to rotate save slots if needed)
    private void AutoSave(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= saveSlotNames.Length)
        {
            Debug.LogError("Invalid slot index.");
            return;
        }

        // Save player position
        PlayerPrefs.SetFloat(saveSlotNames[slotIndex] + "_PosX", transform.position.x);
        PlayerPrefs.SetFloat(saveSlotNames[slotIndex] + "_PosY", transform.position.y);
        PlayerPrefs.SetFloat(saveSlotNames[slotIndex] + "_PosZ", transform.position.z);

        // Save inventory items (using their enum integer values)
        PlayerPrefs.SetInt(saveSlotNames[slotIndex] + "_InventorySize", inventory.Count);

        for (int i = 0; i < inventory.Count; i++)
        {
            PlayerPrefs.SetInt(saveSlotNames[slotIndex] + "_Item_" + i, (int)inventory[i]);
        }

        // Save the data to disk
        PlayerPrefs.Save();

        Debug.Log("Auto-saved to " + saveSlotNames[slotIndex]);
    }

    // Function to load game data from a specific slot (position and inventory)
    public void LoadGame(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= saveSlotNames.Length)
        {
            Debug.LogError("Invalid slot index.");
            return;
        }

        // Load player position
        float posX = PlayerPrefs.GetFloat(saveSlotNames[slotIndex] + "_PosX", 0f);
        float posY = PlayerPrefs.GetFloat(saveSlotNames[slotIndex] + "_PosY", 0f);
        float posZ = PlayerPrefs.GetFloat(saveSlotNames[slotIndex] + "_PosZ", 0f);
        transform.position = new Vector3(posX, posY, posZ);

        // Load inventory
        int inventorySize = PlayerPrefs.GetInt(saveSlotNames[slotIndex] + "_InventorySize", 0);
        inventory.Clear();

        for (int i = 0; i < inventorySize; i++)
        {
            int itemValue = PlayerPrefs.GetInt(saveSlotNames[slotIndex] + "_Item_" + i, 0);
            inventory.Add((InventoryItem)itemValue);  // Convert the stored integer back to the enum
        }

        Debug.Log("Game loaded from " + saveSlotNames[slotIndex]);
    }

    // Clear a specific save slot
    public void ClearSaveSlot(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= saveSlotNames.Length)
        {
            Debug.LogError("Invalid slot index.");
            return;
        }

        // Clear player position
        PlayerPrefs.DeleteKey(saveSlotNames[slotIndex] + "_PosX");
        PlayerPrefs.DeleteKey(saveSlotNames[slotIndex] + "_PosY");
        PlayerPrefs.DeleteKey(saveSlotNames[slotIndex] + "_PosZ");

        // Clear inventory
        int inventorySize = PlayerPrefs.GetInt(saveSlotNames[slotIndex] + "_InventorySize", 0);
        for (int i = 0; i < inventorySize; i++)
        {
            PlayerPrefs.DeleteKey(saveSlotNames[slotIndex] + "_Item_" + i);
        }

        PlayerPrefs.DeleteKey(saveSlotNames[slotIndex] + "_InventorySize");

        PlayerPrefs.Save();
        Debug.Log("Save slot " + saveSlotNames[slotIndex] + " cleared.");
    }
}