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

    void Update()
    {
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, raycastDistance, wandLayer))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                endCutsceneCanvas.enabled = true;
                endCutsceneVideo.gameObject.SetActive(true);
            }
        }
    }
}
