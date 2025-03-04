
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
            }
            else
            {
                ResumeGame();
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
