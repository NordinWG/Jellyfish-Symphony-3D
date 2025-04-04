using UnityEngine;

public class EndCutsceneActivation : MonoBehaviour
{
    [Header("Canvas References")]
    public Canvas endCutsceneCanvas;
    public GameObject endCutsceneVideo;

    [Header("Raycast Settings")]
    public Camera playerCamera;
    public float raycastDistance;
    public LayerMask wandLayer;

    [Header("Item Requirement")]
    public Item requiredItem; // Assign the Staff item here in the Inspector

    void Update()
    {
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, raycastDistance, wandLayer))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                // Check if the player has the required item (staff)
                if (Inventory.instance.HasItem(requiredItem))
                {
                    Debug.Log("Player has the staff — playing end cutscene.");
                    endCutsceneCanvas.enabled = true;
                    endCutsceneVideo.SetActive(true);
                }
                else
                {
                    Debug.Log("Player doesn't have the staff yet.");
                }
            }
        }
    }
}
