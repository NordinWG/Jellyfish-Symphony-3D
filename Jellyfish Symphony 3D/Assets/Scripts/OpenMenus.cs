
using UnityEngine;

public class OpenMenus : MonoBehaviour
{
    public Canvas mainmenu;
	public Canvas pausemenu;
	public Canvas saveLoad;
    public Canvas inventory;

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
