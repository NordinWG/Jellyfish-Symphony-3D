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

    public Button MainMenuQUIT;
    public Button PauseMainMenuB;
    public PlayerMovement playerMovement;


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
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
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

        if (Input.GetKeyDown(KeyCode.Escape) && inventory.enabled && !pausemenu.enabled && !mainmenu.enabled && !saveLoad.enabled && !endCutscene.enabled)
        {
            inventory.enabled = false;
            inventoryUI.ToggleInventory();
            itemDescription.SetActive(false);
        }

        DisableButtonsForInactiveCanvases();
    }

    void PauseGame()
    {
        pausemenu.enabled = true;
        Time.timeScale = 0;
        playerMovement.SetPauseState(true);
    }

    void ResumeGame()
    {
        pausemenu.enabled = false;
        Time.timeScale = 1;
        playerMovement.SetPauseState(false);
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

    void DisableButtonsForInactiveCanvases()
    {
        DisableButtonsInCanvas(mainmenu);
        DisableButtonsInCanvas(pausemenu);
        DisableButtonsInCanvas(saveLoad);
        DisableButtonsInCanvas(inventory);
        DisableButtonsInCanvas(endCutscene);
    }

    void DisableButtonsInCanvas(Canvas canvas)
    {
        if (canvas != null && !canvas.enabled)
        {
            Button[] buttons = canvas.GetComponentsInChildren<Button>(true);
            foreach (Button button in buttons)
            {
                button.interactable = false;
            }
        }
        else if (canvas != null && canvas.enabled)
        {
            Button[] buttons = canvas.GetComponentsInChildren<Button>(true);
            foreach (Button button in buttons)
            {
                button.interactable = true;
            }
        }
    }
}
