using UnityEngine;

public class InventoryToggle : MonoBehaviour
{
    public InventoryUI inventoryUI;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            Debug.Log("InventoryToggle: 'I' key pressed. Toggling inventory.");
            inventoryUI.ToggleInventory();
        }
    }
}