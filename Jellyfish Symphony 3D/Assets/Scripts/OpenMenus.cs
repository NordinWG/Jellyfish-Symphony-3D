
using UnityEngine;

public class OpenMenus : MonoBehaviour
{
    public Canvas mainmenu;
	public Canvas pausemenu;
	public Canvas saveLoad;
    public Canvas inventory;
    public InventoryUI inventoryUI;

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

        if (Input.GetKeyDown(KeyCode.I) && !pausemenu.enabled && !mainmenu.enabled && !inventory.enabled && !saveLoad.enabled)
        {
            if (inventory != null && inventoryUI != null)
            {
                bool isInventoryOpen = inventory.enabled;
                inventoryUI.ToggleInventory();
                inventory.enabled = !isInventoryOpen;
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
