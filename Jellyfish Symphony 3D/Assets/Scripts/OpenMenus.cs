using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OpenMenus : MonoBehaviour
{
    public Canvas mainmenu;
    public Canvas pausemenu;
    public Canvas saveLoad;
    public Canvas inventory;
    public GameObject itemDescription;
    public InventoryUI inventoryUI;

    void Update()
    {
        // Pause Menu Toggle (Escape Key)
        if (Input.GetKeyDown(KeyCode.Escape) && !pausemenu.enabled && !mainmenu.enabled && !inventory.enabled && !saveLoad.enabled)
        {
            if (!pausemenu.enabled)
            {
                print("aA");
                PauseGame();
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                print("bB");
                ResumeGame();
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }

        // Inventory Toggle (I Key)
        if (Input.GetKeyDown(KeyCode.I) && !pausemenu.enabled && !mainmenu.enabled && !saveLoad.enabled)
        {
            bool isInventoryOpen = inventory.enabled;

            if (!isInventoryOpen)
            {
                Debug.Log("Opening Inventory...");
                inventoryUI.ToggleInventory();
                inventory.enabled = true;
                itemDescription.SetActive(true);
            }
            else
            {
                Debug.Log("Closing Inventory...");
                inventory.enabled = false;
                inventoryUI.ToggleInventory();
                itemDescription.SetActive(false);
            }
        }
    }

    void PauseGame()
    {
        pausemenu.enabled = true;
    }

    void ResumeGame()
    {
        pausemenu.enabled = false;
    }
}
