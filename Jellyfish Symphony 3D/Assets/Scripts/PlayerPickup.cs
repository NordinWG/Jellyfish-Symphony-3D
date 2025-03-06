using UnityEngine;
using TMPro;

public class PlayerPickup : MonoBehaviour
{
    public Camera playerCamera;
    public float raycastDistance;
    public LayerMask pickupLayer;

    public TextMeshProUGUI pickupPromptText;

    private ItemPickup currentItem;

    void Update()
    {
        HandleItemRaycast();
        HandleItemPickup();
    }

    void HandleItemRaycast()
    {
        RaycastHit hit;
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, raycastDistance, pickupLayer))
        {
            if (hit.collider.CompareTag("Item"))
            {
                ItemPickup itemPickup = hit.collider.GetComponent<ItemPickup>();
                if (itemPickup != null)
                {
                    currentItem = itemPickup;
                    ShowPickupPrompt(true);
                }
            }
            else
            {
                ShowPickupPrompt(false);
                currentItem = null;
            }
        }
        else
        {
            ShowPickupPrompt(false);
            currentItem = null;
        }
    }

    void ShowPickupPrompt(bool show)
    {
        if (pickupPromptText != null)
        {
            pickupPromptText.gameObject.SetActive(show);
            pickupPromptText.text = show ? "Press E to pick up" : "";
        }
    }

    void HandleItemPickup()
    {
        if (currentItem != null && Input.GetKeyDown(KeyCode.E))
        {
            bool added = Inventory.instance.AddItem(currentItem.item, currentItem.quantity);

            if (added)
            {
                Destroy(currentItem.gameObject);
                ShowPickupPrompt(false);
                currentItem = null;
            }
        }
    }
}