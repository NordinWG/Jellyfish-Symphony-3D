using System.Collections.Generic;
using UnityEngine;

public class SaveLoadSystem : MonoBehaviour

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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    

    {
        // Check for user input to trigger save/load actions

        // Save the game when the player presses the "S" key
        if (Input.GetKeyDown(KeyCode.S))
        {
            SaveGame(0);  // Save to the first save slot
            Debug.Log("Game saved!");
        }

        // Load the game when the player presses the "L" key
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadGame(0);  // Load from the first save slot
            Debug.Log("Game loaded!");
        }

        // Clear save slot when the player presses the "C" key
        if (Input.GetKeyDown(KeyCode.C))
        {
            ClearSaveSlot(0);  // Clear the first save slot
            Debug.Log("Save slot cleared!");
        }
    }

    // Save game data to a specific slot (position and inventory)
    public void SaveGame(int slotIndex)
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

        PlayerPrefs.Save();
        Debug.Log("Game saved to " + saveSlotNames[slotIndex]);
    }

    // Load game data from a specific slot (position and inventory)
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



