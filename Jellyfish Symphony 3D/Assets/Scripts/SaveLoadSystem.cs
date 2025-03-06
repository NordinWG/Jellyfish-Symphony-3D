using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SaveLoadSystem : MonoBehaviour
{
    // Inventory systeem
    public enum InventoryItem { None, Drownie, Shell, Staff }
    public List<InventoryItem> inventory = new List<InventoryItem>();

    // Save slot namen
    private string[] saveSlotNames = { "SaveSlot1", "SaveSlot2", "SaveSlot3" };

    // UI elementen voor het save/load systeem
    public GameObject saveLoadCanvas;
    public GameObject savePanel;
    public GameObject loadPanel;
    public TMP_Text[] saveSlotTexts;
    public TMP_Text[] loadSlotTexts;
    public Button[] saveButtons;
    public Button[] loadButtons;
    public Button saveButton;
    public Button loadButton;

    // Huidige geladen slot weergave
    public TMP_Text currentLoadedSlotText;

    // Autosave UI en instellingen
    public GameObject autoSaveCanvas;
    public GameObject autoSaveIcon;
    public TMP_Text autoSaveText;
    private float autoSaveInterval = 300f; // Autosave elke 5 minuten
    private float autoSaveTimer;

    private int lastLoadedSlotIndex = -1; // Houdt bij welk slot geladen is
    private bool isAutoSaving = false;

    void Start()
    {
        UpdateSaveSlotUI();
        autoSaveTimer = autoSaveInterval;

        // Zorg ervoor dat de autosave UI correct is ingesteld
        if (autoSaveCanvas != null)
        {
            autoSaveCanvas.SetActive(true);
            autoSaveIcon.SetActive(false);
            autoSaveText.text = "";
        }
    }

    void Update()
    {
        // Timer voor autosave
        autoSaveTimer -= Time.deltaTime;

        // Start autosave als de timer op 0 is en er een slot geladen is
        if (autoSaveTimer <= 0f && lastLoadedSlotIndex != -1)
        {
            AutoSave(lastLoadedSlotIndex);
            autoSaveTimer = autoSaveInterval;
        }

        // Handmatig autosaven met 'S'
        if (Input.GetKeyDown(KeyCode.S) && lastLoadedSlotIndex != -1)
        {
            AutoSave(lastLoadedSlotIndex);
        }

        // Laat het autosave icoon roteren als autosaving actief is
        if (isAutoSaving)
        {
            autoSaveIcon.transform.Rotate(Vector3.forward * -180 * Time.deltaTime);
        }

        // Verwijder alle saves met 'End' toets
        if (Input.GetKeyDown(KeyCode.End))
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
            lastLoadedSlotIndex = -1;
            currentLoadedSlotText.text = "Current Loaded Slot: None";
            UpdateSaveSlotUI();
            Debug.Log("All saves deleted");
        }
    }

    // Spel opslaan
    public void SaveGame(int slotIndex)
    {
        string saveName = "World " + (slotIndex + 1);
        PlayerPrefs.SetString(saveSlotNames[slotIndex] + "_Name", saveName);
        PlayerPrefs.Save();
        Debug.Log("Game saved in slot " + (slotIndex + 1));
        UpdateSaveSlotUI();
    }

    // Spel laden
    public void LoadGame(int slotIndex)
    {
        string saveName = PlayerPrefs.GetString(saveSlotNames[slotIndex] + "_Name", "Empty");
        if (saveName == "Empty")
        {
            Debug.Log("No save found in slot " + (slotIndex + 1));
            return;
        }

        PlayerPrefs.Save();
        Debug.Log("Game loaded from slot " + (slotIndex + 1));
        UpdateSaveSlotUI();

        lastLoadedSlotIndex = slotIndex;
        currentLoadedSlotText.text = "Current Loaded Slot: " + saveName;

        // Sluit de save/load UI na het laden
        CloseSaveLoadCanvas();

        autoSaveIcon.SetActive(false);
        autoSaveText.text = "";
    }

    // Autosave functie
    private void AutoSave(int slotIndex)
    {
        string saveName = PlayerPrefs.GetString(saveSlotNames[slotIndex] + "_Name", "Empty");
        if (saveName != "Empty")
        {
            if (autoSaveCanvas != null)
            {
                autoSaveIcon.SetActive(true);
                autoSaveText.text = "Autosaving...";
            }

            isAutoSaving = true;

            SaveGame(slotIndex);

            Debug.Log("Auto-save triggered for slot " + (slotIndex + 1));

            StartCoroutine(HideAutoSaveUI());
        }
    }

    // Verberg autosave UI na een korte tijd
    private IEnumerator HideAutoSaveUI()
    {
        yield return new WaitForSeconds(4f);
        autoSaveIcon.SetActive(false);
        autoSaveText.text = "";
        isAutoSaving = false;
    }

    // Werk de UI bij met de huidige save slots
    void UpdateSaveSlotUI()
    {
        for (int i = 0; i < saveSlotNames.Length; i++)
        {
            string saveName = PlayerPrefs.GetString(saveSlotNames[i] + "_Name", "Empty");
            saveSlotTexts[i].text = saveName;
            loadSlotTexts[i].text = saveName;

            Color buttonColor = saveName == "Empty" ? new Color(0.75f, 0.75f, 0.75f) : Color.white;
            saveButtons[i].image.color = buttonColor;
            loadButtons[i].image.color = buttonColor;
        }
    }

    // Open het save-menu
    public void OpenSavePanel()
    {
        savePanel.SetActive(true);
        loadPanel.SetActive(false);
        saveButton.gameObject.SetActive(false);
        loadButton.gameObject.SetActive(false);
        currentLoadedSlotText.gameObject.SetActive(true);
        Debug.Log("Opened Save Panel");
    }

    // Open het load-menu
    public void OpenLoadPanel()
    {
        loadPanel.SetActive(true);
        savePanel.SetActive(false);
        saveButton.gameObject.SetActive(false);
        loadButton.gameObject.SetActive(false);
        currentLoadedSlotText.gameObject.SetActive(true);
        Debug.Log("Opened Load Panel");
    }

    // Sluit de save/load panelen
    public void ClosePanels()
    {
        savePanel.SetActive(false);
        loadPanel.SetActive(false);
        saveButton.gameObject.SetActive(true);
        loadButton.gameObject.SetActive(true);
        currentLoadedSlotText.gameObject.SetActive(false);
        Debug.Log("Closed Save/Load Panels");
    }

    // Sluit de gehele save/load UI
    public void CloseSaveLoadCanvas()
    {
        if (saveLoadCanvas != null)
        {
            saveLoadCanvas.SetActive(false);
            Debug.Log("Save/Load UI closed");
        }
    }
}