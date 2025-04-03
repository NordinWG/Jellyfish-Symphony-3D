using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SaveLoadSystem : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public PlayerPickup playerPickup;
    public List<GameObject> pickupItems;

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

    private int currentSlotIndex = -1;

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

        if (Input.GetKeyDown(KeyCode.S) && currentSlotIndex != -1)
        {
            AutoSave();
        }

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

    public void OpenSaveLoadPanel()
    {
        saveLoadCanvas.enabled = true;
        UpdateSaveSlotUI();
    }

    public void SelectSlot(int slotIndex)
    {
        currentSlotIndex = slotIndex;
        string saveName = PlayerPrefs.GetString(saveSlotNames[slotIndex] + "_Name", "Empty");
        if (saveName == "Empty")
        {
            SaveGame(slotIndex);
        }
        else
        {
            LoadGame(slotIndex);
        }
    }

    public void LoadGame(int slotIndex)
    {
        string saveName = PlayerPrefs.GetString(saveSlotNames[slotIndex] + "_Name", "Empty");
        if (saveName != "Empty")
        {
            currentSlotIndex = slotIndex;

            float x = PlayerPrefs.GetFloat(saveSlotNames[slotIndex] + "_PlayerX");
            float y = PlayerPrefs.GetFloat(saveSlotNames[slotIndex] + "_PlayerY");
            float z = PlayerPrefs.GetFloat(saveSlotNames[slotIndex] + "_PlayerZ");
            playerMovement.transform.position = new Vector3(x, y, z);

            Inventory.instance.inventorySlots.Clear();
            int slotCount = PlayerPrefs.GetInt(saveSlotNames[slotIndex] + "_SlotCount", 0);
            Debug.Log($"Loading {slotCount} inventory slots for slot {slotIndex}");
            for (int i = 0; i < slotCount; i++)
            {
                string itemName = PlayerPrefs.GetString(saveSlotNames[slotIndex] + "_ItemName" + i);
                int quantity = PlayerPrefs.GetInt(saveSlotNames[slotIndex] + "_ItemQty" + i);
                Item item = Resources.Load<Item>("Items/" + itemName);
                if (item != null)
                {
                    Inventory.instance.inventorySlots.Add(new InventorySlot(item, quantity));
                    Debug.Log($"Loaded item: {itemName}, Quantity: {quantity}");
                }
                else
                {
                    Debug.LogWarning($"Failed to load item: {itemName} from Resources/Items/");
                }
            }
            Inventory.instance.onInventoryChangedCallback?.Invoke();

            for (int i = 0; i < pickupItems.Count; i++)
            {
                if (pickupItems[i] != null)
                {
                    bool isActive = PlayerPrefs.GetInt(saveSlotNames[slotIndex] + "_Pickup" + i, 1) == 1;
                    pickupItems[i].SetActive(isActive);
                }
            }

            Debug.Log("Game Loaded from slot " + (slotIndex + 1));
            CloseSaveLoadCanvas();
        }
    }

    public void SaveGame(int slotIndex)
    {
        PlayerPrefs.SetString(saveSlotNames[slotIndex] + "_Name", "World " + (slotIndex + 1));

        PlayerPrefs.SetFloat(saveSlotNames[slotIndex] + "_PlayerX", playerMovement.transform.position.x);
        PlayerPrefs.SetFloat(saveSlotNames[slotIndex] + "_PlayerY", playerMovement.transform.position.y);
        PlayerPrefs.SetFloat(saveSlotNames[slotIndex] + "_PlayerZ", playerMovement.transform.position.z);

        PlayerPrefs.SetInt(saveSlotNames[slotIndex] + "_SlotCount", Inventory.instance.inventorySlots.Count);
        Debug.Log($"Saving {Inventory.instance.inventorySlots.Count} inventory slots for slot {slotIndex}");
        for (int i = 0; i < Inventory.instance.inventorySlots.Count; i++)
        {
            string itemName = Inventory.instance.inventorySlots[i].item.itemName;
            int quantity = Inventory.instance.inventorySlots[i].quantity;
            PlayerPrefs.SetString(saveSlotNames[slotIndex] + "_ItemName" + i, itemName);
            PlayerPrefs.SetInt(saveSlotNames[slotIndex] + "_ItemQty" + i, quantity);
            Debug.Log($"Saved item: {itemName}, Quantity: {quantity}");
        }

        for (int i = 0; i < pickupItems.Count; i++)
        {
            PlayerPrefs.SetInt(saveSlotNames[slotIndex] + "_Pickup" + i,
                (pickupItems[i] != null && pickupItems[i].activeSelf) ? 1 : 0);
        }

        PlayerPrefs.Save();
        currentSlotIndex = slotIndex;
        Debug.Log("Game Saved to slot " + (slotIndex + 1));
        CloseSaveLoadCanvas();
    }

    private void AutoSave()
    {
        if (currentSlotIndex == -1) return;

        autoSaveIcon.SetActive(true);
        autoSaveText.text = "Autosaving...";
        StartCoroutine(SpinAutoSaveIcon());
        SaveGame(currentSlotIndex);
        StartCoroutine(HideAutoSaveUI());
    }

    public void OnSaveButtonClicked()
    {
        if (currentSlotIndex != -1)
        {
            autoSaveIcon.SetActive(true);
            autoSaveText.text = "Saving...";
            StartCoroutine(SpinAutoSaveIcon());
            SaveGame(currentSlotIndex);
            StartCoroutine(HideAutoSaveUI());
        }
    }

    private void DeleteAllSaves()
    {
        for (int i = 0; i < saveSlotNames.Length; i++)
        {
            PlayerPrefs.DeleteKey(saveSlotNames[i] + "_Name");
            PlayerPrefs.DeleteKey(saveSlotNames[i] + "_PlayerX");
            PlayerPrefs.DeleteKey(saveSlotNames[i] + "_PlayerY");
            PlayerPrefs.DeleteKey(saveSlotNames[i] + "_PlayerZ");
            PlayerPrefs.DeleteKey(saveSlotNames[i] + "_SlotCount");

            for (int j = 0; j < 100; j++)
            {
                PlayerPrefs.DeleteKey(saveSlotNames[i] + "_ItemName" + j);
                PlayerPrefs.DeleteKey(saveSlotNames[i] + "_ItemQty" + j);
                PlayerPrefs.DeleteKey(saveSlotNames[i] + "_Pickup" + j);
            }
        }
        PlayerPrefs.Save();
        UpdateSaveSlotUI();
    }

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

    private IEnumerator SpinAutoSaveIcon()
    {
        while (autoSaveIcon.activeSelf)
        {
            autoSaveIcon.transform.Rotate(0, 0, -180 * Time.deltaTime);
            yield return null;
        }
    }

    private IEnumerator HideAutoSaveUI()
    {
        yield return new WaitForSeconds(2f);
        autoSaveIcon.SetActive(false);
        autoSaveText.text = "";
    }

    public void CloseSaveLoadCanvas()
    {
        saveLoadCanvas.enabled = false;
    }
}