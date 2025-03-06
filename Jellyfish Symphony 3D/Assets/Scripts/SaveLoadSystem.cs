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

    public Canvas saveLoadCanvas;
    public TMP_Text[] saveSlotTexts;
    public Button[] slotButtons;
    public Button saveButton;

    public GameObject autoSaveCanvas;
    public GameObject autoSaveIcon;
    public TMP_Text autoSaveText;
    private float autoSaveInterval = 300f;
    private float autoSaveTimer;

    private int currentSlotIndex = -1;  // -1 indicates no slot selected yet

    void Start()
    {
        UpdateSaveSlotUI();
        autoSaveTimer = autoSaveInterval;
        autoSaveCanvas?.SetActive(true);
        autoSaveIcon?.SetActive(false);
        autoSaveText.text = "";
    }

    void Update()
    {
        autoSaveTimer -= Time.deltaTime;

        // Trigger autosave on the 'S' key (if a game is loaded)
        if (Input.GetKeyDown(KeyCode.S) && currentSlotIndex != -1)
        {
            AutoSave();
        }

        // Trigger delete all saves on the 'End' key
        if (Input.GetKeyDown(KeyCode.End))
        {
            DeleteAllSaves();
        }

        if (autoSaveTimer <= 0f && currentSlotIndex != -1)
        {
            AutoSave();
            autoSaveTimer = autoSaveInterval;
        }
    }

    // Open the save/load panel
    public void OpenSaveLoadPanel()
    {
        saveLoadCanvas.enabled = true;
        UpdateSaveSlotUI();
    }

    // Select the save slot
    public void SelectSlot(int slotIndex)
    {
        currentSlotIndex = slotIndex; // Set current slot index to the selected slot
        string saveName = PlayerPrefs.GetString(saveSlotNames[slotIndex] + "_Name", "Empty");
        if (saveName == "Empty")
        {
            SaveGame(slotIndex);  // Save if the slot is empty
        }
        else
        {
            LoadGame(slotIndex);  // Load the game if the slot is not empty
        }
    }

    // Load game from the selected slot
    public void LoadGame(int slotIndex)
    {
        string saveName = PlayerPrefs.GetString(saveSlotNames[slotIndex] + "_Name", "Empty");
        if (saveName != "Empty")
        {
            currentSlotIndex = slotIndex;
            Debug.Log("Game Loaded from slot " + (slotIndex + 1));
            CloseSaveLoadCanvas();
        }
        else
        {
            Debug.Log("No game found in slot " + (slotIndex + 1));
        }
    }

    // Save game to the selected slot
    public void SaveGame(int slotIndex)
    {
        PlayerPrefs.SetString(saveSlotNames[slotIndex] + "_Name", "World " + (slotIndex + 1));
        PlayerPrefs.Save();
        currentSlotIndex = slotIndex; // Save to the selected slot
        Debug.Log("Game Saved to slot " + (slotIndex + 1));
        UpdateSaveSlotUI();
        CloseSaveLoadCanvas();
    }

    // Autosave function
    private void AutoSave()
    {
        if (currentSlotIndex == -1) return;

        // Show autosave UI elements (like the "Saving..." message)
        autoSaveIcon.SetActive(true);
        autoSaveText.text = "Autosaving...";

        // Start spinning the auto-save icon continuously
        StartCoroutine(SpinAutoSaveIcon());

        // Save the game to the current slot
        SaveGame(currentSlotIndex);

        // Hide the "Saving..." UI after the save
        StartCoroutine(HideAutoSaveUI());
    }

    // Triggered by manual save button in pause menu
    public void OnSaveButtonClicked()
    {
        if (currentSlotIndex != -1)
        {
            // Show saving UI elements (like the "Saving..." message)
            autoSaveIcon.SetActive(true);
            autoSaveText.text = "Saving...";

            // Start spinning the auto-save icon continuously
            StartCoroutine(SpinAutoSaveIcon());

            // Save the game to the current slot
            SaveGame(currentSlotIndex);

            // Hide the "Saving..." UI after the save
            StartCoroutine(HideAutoSaveUI());
        }
        else
        {
            Debug.LogWarning("No save slot selected");
        }
    }

    // Delete all saves (called by pressing the 'End' key)
    private void DeleteAllSaves()
    {
        for (int i = 0; i < saveSlotNames.Length; i++)
        {
            PlayerPrefs.DeleteKey(saveSlotNames[i] + "_Name");
            Debug.Log("Deleted save data from slot " + (i + 1));
        }
        PlayerPrefs.Save();
        UpdateSaveSlotUI(); // Update UI to reflect changes
    }

    // Update the save slot UI
    void UpdateSaveSlotUI()
    {
        for (int i = 0; i < saveSlotNames.Length; i++)
        {
            string saveName = PlayerPrefs.GetString(saveSlotNames[i] + "_Name", "Empty");
            saveSlotTexts[i].text = saveName;

            Color buttonColor = saveName == "Empty" ? new Color(0.75f, 0.75f, 0.75f) : Color.white;
            slotButtons[i].image.color = buttonColor;
            slotButtons[i].interactable = true;
        }
    }

    // Spin the auto-save icon continuously at -180 degrees per second
    private IEnumerator SpinAutoSaveIcon()
    {
        while (autoSaveIcon.activeSelf)  // Keep spinning while the icon is active
        {
            autoSaveIcon.transform.Rotate(0, 0, -180 * Time.deltaTime);  // Rotate the icon by -180 degrees per second
            yield return null;
        }
    }

    // Hide the auto-save UI after the save completes
    private IEnumerator HideAutoSaveUI()
    {
        yield return new WaitForSeconds(2f);  // Wait for 4 seconds to simulate saving time

        // Reset the auto-save UI after the save completes
        autoSaveIcon.SetActive(false);
        autoSaveText.text = "";  // Remove "Saving..." text after the save
    }

    // Close the save/load canvas
    public void CloseSaveLoadCanvas()
    {
        saveLoadCanvas.enabled = false;
    }
}
