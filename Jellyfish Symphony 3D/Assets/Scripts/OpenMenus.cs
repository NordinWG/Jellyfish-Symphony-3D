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
    public Canvas endCutscene;
    public GameObject itemDescription;
    public InventoryUI inventoryUI;
    public Canvas onscreenUI;


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
        if (Input.GetKeyDown(KeyCode.Escape) && !mainmenu.enabled && !saveLoad.enabled && !inventory.enabled && !endCutscene.enabled)
        {
            if (!pausemenu.enabled)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }
        if (pausemenu.enabled || mainmenu.enabled || saveLoad.enabled || inventory.enabled || endCutscene.enabled)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            onscreenUI.enabled = false;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            onscreenUI.enabled = true;
        }

        if (Input.GetKeyDown(KeyCode.I) && !pausemenu.enabled && !mainmenu.enabled && !saveLoad.enabled && !endCutscene.enabled)
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
