using UnityEngine;

public class InventoryToggle : MonoBehaviour
{
    public InventoryUI inventoryUI;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryUI.ToggleInventory();
        }
    }
}
