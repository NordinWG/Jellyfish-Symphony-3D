using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SaveLoadSystem : MonoBehaviour
{
    public enum InventoryItem { None, Drownie, Shell, Staff }
    public List<InventoryItem> inventory = new List<InventoryItem>();

    private string[] saveSlotNames = { "SaveSlot1", "SaveSlot2", "SaveSlot3" };
    public GameObject savePanel;
    public GameObject loadPanel;
    public TMP_Text[] saveSlotTexts;
    public TMP_Text[] loadSlotTexts;
    public Button[] saveButtons;
    public Button[] loadButtons;
    public Button saveButton;
    public Button loadButton;

    // New TMP_Text for displaying the current loaded slot outside of the Save/Load panels
    public TMP_Text currentLoadedSlotText;

    // AutoSave references
    public GameObject autoSaveIcon;  // The icon that rotates during autosave
    public TMP_Text autoSaveText;    // The text displaying "Autosaving..."
    private float autoSaveInterval = 10f; // 10 seconds in seconds for testing
    private float autoSaveTimer;

    private int lastLoadedSlotIndex = -1;  // Store the index of the last loaded slot
    private bool isAutoSaving = false;     // Flag to indicate if autosaving is in progress

    void Start()
    {
        UpdateSaveSlotUI(); // Initialize UI on start
        autoSaveTimer = autoSaveInterval; // Start the timer at the full interval
    }

    void Update()
    {
        // Update the auto-save timer
        autoSaveTimer -= Time.deltaTime;

        // If the timer runs out, trigger autosave
        if (autoSaveTimer <= 0f && lastLoadedSlotIndex != -1)
        {
            AutoSave(lastLoadedSlotIndex);  // Autosave the loaded slot
            autoSaveTimer = autoSaveInterval; // Reset the timer
        }

        if (Input.GetKeyDown(KeyCode.S) && lastLoadedSlotIndex != -1)
        {
            AutoSave(lastLoadedSlotIndex);  // Autosave the loaded slot when pressing 'S'
        }

        // Handle rotating the icon if it's active
        if (isAutoSaving)
        {
            autoSaveIcon.transform.Rotate(Vector3.forward * -180 * Time.deltaTime); // Rotate the icon on Z-axis
        }

        // Delete all saved data when pressing 'End' key (for testing purposes)
        if (Input.GetKeyDown(KeyCode.End))
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
            UpdateSaveSlotUI();
            Debug.Log("All saves deleted"); // Debug message
        }
    }

    // Saves the game to a specific slot
    public void SaveGame(int slotIndex)
    {
        string saveName = "World " + (slotIndex + 1);
        PlayerPrefs.SetString(saveSlotNames[slotIndex] + "_Name", saveName);
        PlayerPrefs.Save();
        Debug.Log("Game saved in slot " + (slotIndex + 1)); // Debug message
        UpdateSaveSlotUI(); // Update UI after saving
    }

    // Loads the game from a specific slot
    public void LoadGame(int slotIndex)
    {
        string saveName = PlayerPrefs.GetString(saveSlotNames[slotIndex] + "_Name", "Empty");
        if (saveName == "Empty")
        {
            Debug.Log("No save found in slot " + (slotIndex + 1)); // Debug message
            return;
        }
        PlayerPrefs.Save();
        Debug.Log("Game loaded from slot " + (slotIndex + 1)); // Debug message
        UpdateSaveSlotUI();

        // Set the loaded slot index and show the current loaded slot text
        lastLoadedSlotIndex = slotIndex;
        currentLoadedSlotText.text = "Current Loaded Slot: " + saveName;

        // Ensure the autosave icon and text are hidden if they are already visible
        autoSaveIcon.SetActive(false);
        autoSaveText.text = "";
    }

    // Autosave function
    private void AutoSave(int slotIndex)
    {
        // Display the autosaving UI if a valid slot is loaded
        string saveName = PlayerPrefs.GetString(saveSlotNames[slotIndex] + "_Name", "Empty");
        if (saveName != "Empty")  // Only autosave if a valid slot is loaded
        {
            // Show the rotating icon and "Autosaving..." text
            autoSaveIcon.SetActive(true);
            autoSaveText.text = "Autosaving...";  // Set the text once, don't update it
            isAutoSaving = true;  // Flag to start the rotation

            // Perform the save operation to the currently loaded slot
            SaveGame(slotIndex); // Save to the loaded slot

            // Log the autosave
            Debug.Log("Auto-save triggered for slot " + (slotIndex + 1)); // Debug message

            // Call the coroutine to hide the autosaving UI after 4 seconds
            StartCoroutine(HideAutoSaveUI());
        }
        else
        {
            // Ensure the autosave UI is hidden if no valid slot is loaded
            autoSaveIcon.SetActive(false);
            autoSaveText.text = "";
        }
    }

    // Coroutine to hide the autosaving UI after a brief period
    private IEnumerator HideAutoSaveUI()
    {
        // Wait for 4 seconds to let the "Autosaving..." message appear
        yield return new WaitForSeconds(4f);

        // Hide the autosaving UI and stop rotation
        autoSaveIcon.SetActive(false);  // Hide the rotating icon
        autoSaveText.text = "";        // Clear the autosaving message
        isAutoSaving = false;          // Stop rotating the icon
    }

    // Updates the UI to reflect the save slot states
    void UpdateSaveSlotUI()
    {
        for (int i = 0; i < saveSlotNames.Length; i++)
        {
            string saveName = PlayerPrefs.GetString(saveSlotNames[i] + "_Name", "Empty");
            saveSlotTexts[i].text = saveName;
            loadSlotTexts[i].text = saveName;

            // Set button color: gray if empty, white if used
            Color buttonColor = saveName == "Empty" ? new Color(0.75f, 0.75f, 0.75f) : Color.white;
            saveButtons[i].image.color = buttonColor;
            loadButtons[i].image.color = buttonColor;
        }
    }

    // Opens the save panel and hides the load panel
    public void OpenSavePanel()
    {
        savePanel.SetActive(true);
        loadPanel.SetActive(false);
        saveButton.gameObject.SetActive(false);
        loadButton.gameObject.SetActive(false);

        // Show the current loaded slot text when Save Panel is open
        currentLoadedSlotText.gameObject.SetActive(true);

        Debug.Log("Opened Save Panel"); // Debug message
    }

    // Opens the load panel and hides the save panel
    public void OpenLoadPanel()
    {
        loadPanel.SetActive(true);
        savePanel.SetActive(false);
        saveButton.gameObject.SetActive(false);
        loadButton.gameObject.SetActive(false);

        // Show the current loaded slot text when Load Panel is open
        currentLoadedSlotText.gameObject.SetActive(true);

        Debug.Log("Opened Load Panel"); // Debug message
    }

    // Closes both panels and restores the save/load buttons
    public void ClosePanels()
    {
        savePanel.SetActive(false);
        loadPanel.SetActive(false);
        saveButton.gameObject.SetActive(true);
        loadButton.gameObject.SetActive(true);

        // Hide the current loaded slot text when both panels are closed
        currentLoadedSlotText.gameObject.SetActive(false);

        Debug.Log("Closed Save/Load Panels"); // Debug message
    }
}
