using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class SaveLoadSystem : MonoBehaviour
{
    public enum InventoryItem { None, Drownie, Shell, Staff }
    public List<InventoryItem> inventory = new List<InventoryItem>();

    private string[] saveSlotNames = { "SaveSlot1", "SaveSlot2", "SaveSlot3" };
    public GameObject saveLoadPanel;
    public TMP_Text[] saveSlotTexts;
    public Button[] saveButtons; // Buttons for each save slot

    public Button saveButton; // Reference to the Save button
    public Button loadButton; // Reference to the Load button

    private int nextSaveSlot = 0; // Track the next available save slot
    private bool isSaving = false; // To avoid saving multiple times in one frame

    void Start()
    {
        UpdateSaveSlotUI(); // Initialize the UI with current save state
    }

    // Save the game data
    public void SaveGame(int slotIndex)
    {
        if (isSaving || nextSaveSlot > saveSlotNames.Length) return;

        isSaving = true; // Block further saving while this action is in progress

        // If all slots are full, do not save and flash the save button red
        if (nextSaveSlot >= saveSlotNames.Length)
        {
            StartCoroutine(IndicateSaveLimit());
            isSaving = false;
            return;
        }

        // Save to the next available slot
        slotIndex = nextSaveSlot;

        string saveName = "World " + (slotIndex + 1); // Name as World 1, World 2, etc.

        // Save the name to PlayerPrefs
        PlayerPrefs.SetString(saveSlotNames[slotIndex] + "_Name", saveName);

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

        // Update nextSaveSlot to the next available slot
        nextSaveSlot = Mathf.Min(nextSaveSlot + 1, saveSlotNames.Length);

        // Update the UI after saving
        UpdateSaveSlotUI();

        Debug.Log("Game saved to " + saveSlotNames[slotIndex]);

        isSaving = false; // Allow new save attempts
    }

    // Load the game data
    public void LoadGame(int slotIndex)
    {
        string saveName = PlayerPrefs.GetString(saveSlotNames[slotIndex] + "_Name", "Empty");
        if (saveName == "Empty")
        {
            Debug.Log("No save found in this slot.");
            return; // Exit if there's no save in this slot
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
            inventory.Add((InventoryItem)itemValue);
        }

        PlayerPrefs.Save();

        // Update the UI after loading
        UpdateSaveSlotUI();

        Debug.Log("Game loaded from " + saveSlotNames[slotIndex]);

        // Update the UI for loaded state (Green color)
        saveButtons[slotIndex].GetComponent<Image>().color = Color.green; // Green when loaded
    }

    // Clear a specific save slot
    public void ClearSaveSlot(int slotIndex)
    {
        // Clear all saved data for the selected slot
        PlayerPrefs.DeleteKey(saveSlotNames[slotIndex] + "_Name");
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

        // After clearing, set nextSaveSlot to the lowest available slot
        nextSaveSlot = Mathf.Min(nextSaveSlot, slotIndex);

        // Update the UI after clearing the slot
        UpdateSaveSlotUI(); // Refresh the UI to reflect changes

        Debug.Log("Save slot " + saveSlotNames[slotIndex] + " cleared.");
    }

    // Update the UI for save slots (button colors and text)
    void UpdateSaveSlotUI()
    {
        for (int i = 0; i < saveSlotNames.Length; i++)
        {
            // Get save name from PlayerPrefs (empty if no save)
            string saveName = PlayerPrefs.GetString(saveSlotNames[i] + "_Name", "Empty");

            // Update the slot text to "World X" or "Empty"
            saveSlotTexts[i].text = saveName == "Empty" ? "Empty" : saveName;

            // Update button color and interactivity based on whether the slot has saved data
            if (saveName == "Empty")
            {
                saveButtons[i].interactable = true; // Enable button for empty slots
                saveButtons[i].GetComponent<Image>().color = Color.white; // Set button color to white
            }
            else
            {
                saveButtons[i].interactable = true; // Enable button for filled slots
                saveButtons[i].GetComponent<Image>().color = Color.white; // Set button color to white
            }
        }
    }


    // Method to open the Save/Load UI
    public void OpenSaveLoadUI()
    {
        saveLoadPanel.SetActive(true);
        UpdateSaveSlotUI(); // Refresh UI when opening
        saveButton.gameObject.SetActive(false); // Hide save button when the save/load panel is open
        loadButton.gameObject.SetActive(false); // Hide load button when the save/load panel is open
    }

    // Method to close the Save/Load UI
    public void CloseSaveLoadUI()
    {
        saveLoadPanel.SetActive(false);
        saveButton.gameObject.SetActive(true); // Show save button when the save/load panel is closed
        loadButton.gameObject.SetActive(true); // Show load button when the save/load panel is closed
    }

    // Coroutine to indicate the save limit is reached by turning the button red and flashing
    private IEnumerator IndicateSaveLimit()
    {
        saveButton.GetComponent<Image>().color = Color.red; // Turn button red
        yield return new WaitForSeconds(0.5f); // Wait for half a second
        saveButton.GetComponent<Image>().color = Color.white; // Reset button color to white
        yield return new WaitForSeconds(0.5f); // Wait for half a second
        saveButton.GetComponent<Image>().color = Color.red; // Turn button red again
        yield return new WaitForSeconds(0.5f); // Wait for another half second
        saveButton.GetComponent<Image>().color = Color.white; // Reset button color to white
    }

    // Method to handle loading a specific save slot (button click)
    public void OnSlotClicked(int slotIndex)
    {
        // Load the game data when a slot is clicked
        LoadGame(slotIndex);

        // Update the UI for loaded state (Green color)
        saveButtons[slotIndex].GetComponent<Image>().color = Color.green; // Green when loaded
    }
}
