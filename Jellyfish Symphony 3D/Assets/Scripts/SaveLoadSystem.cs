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
    public GameObject saveLoadCanvas;
    public GameObject savePanel;
    public GameObject loadPanel;
    public TMP_Text[] saveSlotTexts;
    public TMP_Text[] loadSlotTexts;
    public Button[] saveButtons;
    public Button[] loadButtons;
    public Button saveButton;
    public Button loadButton;

    public TMP_Text currentLoadedSlotText;

    public GameObject autoSaveCanvas;
    public GameObject autoSaveIcon;
    public TMP_Text autoSaveText;
    private float autoSaveInterval = 300f;
    private float autoSaveTimer;

    private int lastLoadedSlotIndex = -1;
    private bool isAutoSaving = false;

    void Start()
    {
        UpdateSaveSlotUI();
        autoSaveTimer = autoSaveInterval;

        if (autoSaveCanvas != null)
        {
            autoSaveCanvas.SetActive(true);
            autoSaveIcon.SetActive(false);
            autoSaveText.text = "";
        }
    }

    void Update()
    {
        autoSaveTimer -= Time.deltaTime;

        if (autoSaveTimer <= 0f && lastLoadedSlotIndex != -1)
        {
            AutoSave(lastLoadedSlotIndex);
            autoSaveTimer = autoSaveInterval;
        }

        if (Input.GetKeyDown(KeyCode.S) && lastLoadedSlotIndex != -1)
        {
            AutoSave(lastLoadedSlotIndex);
        }

        if (isAutoSaving)
        {
            autoSaveIcon.transform.Rotate(Vector3.forward * -180 * Time.deltaTime);
        }

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

    public void SaveGame(int slotIndex)
    {
        string saveName = "World " + (slotIndex + 1);
        PlayerPrefs.SetString(saveSlotNames[slotIndex] + "_Name", saveName);
        PlayerPrefs.Save();
        Debug.Log("Game saved in slot " + (slotIndex + 1));
        UpdateSaveSlotUI();
    }

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

        CloseSaveLoadCanvas();

        autoSaveIcon.SetActive(false);
        autoSaveText.text = "";
    }

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

    private IEnumerator HideAutoSaveUI()
    {
        yield return new WaitForSeconds(4f);

        autoSaveIcon.SetActive(false);
        autoSaveText.text = "";
        isAutoSaving = false;
    }

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

    public void OpenSavePanel()
    {
        savePanel.SetActive(true);
        loadPanel.SetActive(false);
        saveButton.gameObject.SetActive(false);
        loadButton.gameObject.SetActive(false);
        currentLoadedSlotText.gameObject.SetActive(true);

        Debug.Log("Opened Save Panel");
    }

    public void OpenLoadPanel()
    {
        loadPanel.SetActive(true);
        savePanel.SetActive(false);
        saveButton.gameObject.SetActive(false);
        loadButton.gameObject.SetActive(false);
        currentLoadedSlotText.gameObject.SetActive(true);

        Debug.Log("Opened Load Panel");
    }

    public void ClosePanels()
    {
        savePanel.SetActive(false);
        loadPanel.SetActive(false);
        saveButton.gameObject.SetActive(true);
        loadButton.gameObject.SetActive(true);
        currentLoadedSlotText.gameObject.SetActive(false);

        Debug.Log("Closed Save/Load Panels");
    }

    public void CloseSaveLoadCanvas()
    {
        if (saveLoadCanvas != null)
        {
            saveLoadCanvas.SetActive(false);
            Debug.Log("Save/Load UI closed");
        }
    }
}
