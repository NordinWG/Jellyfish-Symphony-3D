using UnityEngine;
using TMPro;

public class PlayerPickup : MonoBehaviour
{
    public float pickupSphereRadius;
    public LayerMask pickupLayer;
    public LayerMask wandPickupLayer;
    private ItemPickup currentItem;

    void Update()
    {
        HandleItemDetection();
        HandleItemPickup();
    }

    void HandleItemDetection()
    {
        if (currentItem != null)
        {
            currentItem.ShowPickupPrompt(false);
            currentItem = null;
        }

        Collider[] hits = Physics.OverlapSphere(transform.position, pickupSphereRadius, pickupLayer | wandPickupLayer);
        
        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Item"))
            {
                ItemPickup itemPickup = hit.GetComponent<ItemPickup>();
                if (itemPickup != null)
                {
                    currentItem = itemPickup;
                    currentItem.ShowPickupPrompt(true);
                    break;
                }
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

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, pickupSphereRadius);
    }
}