using UnityEngine;
using TMPro;  // Importing TextMeshPro namespace

public class PlayerPickup : MonoBehaviour
{
    public Camera playerCamera;
    public float raycastDistance = 5f; // Distance at which you can pick up items
    public LayerMask pickupLayer; // Layer to detect pickup items (set in Inspector)

    public TextMeshProUGUI pickupPromptText;  // Reference to TMP Text for the prompt ("Press E to pick up")

    private ItemPickup currentItem;  // The item the player is looking at

    void Update()
    {
        HandleItemRaycast();
        HandleItemPickup();
    }

    // Raycast to detect the item in front of the player
    void HandleItemRaycast()
    {
        RaycastHit hit;
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition); // Ray from camera to mouse position or center

        if (Physics.Raycast(ray, out hit, raycastDistance, pickupLayer))
        {
            if (hit.collider.CompareTag("Item"))
            {
                // Show prompt to pick up the item
                ItemPickup itemPickup = hit.collider.GetComponent<ItemPickup>();
                if (itemPickup != null)
                {
                    currentItem = itemPickup;
                    ShowPickupPrompt(true);
                }
            }
            else
            {
                // Hide prompt if no item is hit
                ShowPickupPrompt(false);
                currentItem = null;
            }
        }
        else
        {
            // Hide prompt if raycast doesn't hit anything
            ShowPickupPrompt(false);
            currentItem = null;
        }
    }

    // Show or hide the pickup prompt text
    void ShowPickupPrompt(bool show)
    {
        if (pickupPromptText != null)
        {
            pickupPromptText.gameObject.SetActive(show);
            pickupPromptText.text = show ? "Press E to pick up" : "";
        }
    }

    // Handle item pickup with E key
    void HandleItemPickup()
    {
        if (currentItem != null && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log($"Picked up {currentItem.item.itemName}");
            bool added = Inventory.instance.AddItem(currentItem.item, currentItem.quantity);

            if (added)
            {
                Destroy(currentItem.gameObject);  // Destroy the item from the scene
                ShowPickupPrompt(false);  // Hide the prompt
                currentItem = null;  // Reset the current item
            }
            else
            {
                Debug.Log("Inventory full! Cannot pick up item.");
            }
        }
    }
}
