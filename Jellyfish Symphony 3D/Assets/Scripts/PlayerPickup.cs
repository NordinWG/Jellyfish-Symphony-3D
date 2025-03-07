using UnityEngine;
using TMPro;

public class PlayerPickup : MonoBehaviour
{
    public Camera playerCamera;
    public float raycastDistance = 5f;
    public LayerMask pickupLayer;

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
                    currentItem.ShowPickupPrompt(true);
                }
            }
            else
            {
                if (currentItem != null)
                {
                    currentItem.ShowPickupPrompt(false);
                    currentItem = null;
                }
            }
        }
        else
        {
            if (currentItem != null)
            {
                currentItem.ShowPickupPrompt(false);
                currentItem = null;
            }
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
                currentItem.ShowPickupPrompt(false);
                currentItem = null;
            }
        }
    }
}
