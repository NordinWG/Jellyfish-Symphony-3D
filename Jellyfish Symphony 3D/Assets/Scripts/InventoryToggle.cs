using UnityEngine;

public class InventoryToggle : MonoBehaviour
{
    public InventoryUI inventoryUI;
    public Canvas inventory;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (inventory != null && inventoryUI != null)
            {
                bool isInventoryOpen = inventory.enabled;
                inventoryUI.ToggleInventory();
                inventory.enabled = !isInventoryOpen;
            }
        }
    }
}
