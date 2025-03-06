using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using System;
using TMPro;

public class OpenMenus : MonoBehaviour
{
    [Header("Canvas")]
    public Canvas mainmenu;
    public Canvas pausemenu;
    public Canvas saveLoad;
    public Canvas inventory;
    public GameObject itemDescription;
    public InventoryUI inventoryUI;


    [Header("Quit Buttons")]
    public Button MainMenuQUIT;
    public Button PauseMainMenuB;

    void Start()
    {
        MainMenuQUIT.onClick.AddListener(QuitGame);
        PauseMainMenuB.onClick.AddListener(ResetGame);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !pausemenu.enabled && !mainmenu.enabled && !inventory.enabled && !saveLoad.enabled)
        {
            if (!pausemenu.enabled)
            {
                PauseGame();
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                ResumeGame();
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }

        if (Input.GetKeyDown(KeyCode.I) && !pausemenu.enabled && !mainmenu.enabled && !saveLoad.enabled)
        {
            bool isInventoryOpen = inventory.enabled;

            if (!isInventoryOpen)
            {
                inventoryUI.ToggleInventory();
                inventory.enabled = true;
                itemDescription.SetActive(true);
            }
            else
            {
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

    void QuitGame()
    {
        Application.Quit();
    }

    void ResetGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
