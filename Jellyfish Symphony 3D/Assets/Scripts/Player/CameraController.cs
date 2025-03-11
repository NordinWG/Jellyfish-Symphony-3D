using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Camera Settings")]
    public float sensitivityX;
    public float sensitivityY;
    public float minYAngle;
    public float maxYAngle;
    public float distanceFromTarget;

    [Header("Collision Settings")]
    public LayerMask collisionLayers;
    public float collisionOffset;
    public float minDistanceFromTarget;

    [Header("Canvas References")]
    public Canvas mainMenu;
    public Canvas pauseMenu;
    public Canvas saveLoad;
    public Canvas inventory;
    public Canvas endCutscene;

    [Header("References")]
    public Transform target;

    private bool canMove;
    private float currentX;
    private float currentY;

    private void Start()
    {
        if (!target)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }
        Cursor.lockState = CursorLockMode.Locked;

        UpdateCameraPosition(true);
    }

    private void Update()
    {
        UpdateCanMove();
    }

    private void UpdateCanMove()
    {
        canMove = !(mainMenu.enabled || pauseMenu.enabled || saveLoad.enabled || inventory.enabled || endCutscene.enabled);
    }

    private void LateUpdate()
    {
        if (!canMove) return;

        currentX += Input.GetAxisRaw("Mouse X") * sensitivityX;
        currentY -= Input.GetAxisRaw("Mouse Y") * sensitivityY;
        currentY = Mathf.Clamp(currentY, minYAngle, maxYAngle);

        UpdateCameraPosition(false);
    }

    private void UpdateCameraPosition(bool instant)
    {
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        Vector3 direction = rotation * -Vector3.forward;
        Vector3 desiredPosition = target.position + direction * distanceFromTarget;
        Vector3 adjustedPosition = AdjustForCollisions(target.position, desiredPosition, direction);

        transform.position = instant ? adjustedPosition : adjustedPosition;
        transform.LookAt(target.position + Vector3.up * 1f);
    }

    private Vector3 AdjustForCollisions(Vector3 start, Vector3 desired, Vector3 direction)
    {
        RaycastHit hit;
        float maxDistance = Vector3.Distance(start, desired);

        if (Physics.Raycast(start, direction, out hit, maxDistance, collisionLayers))
        {
            float adjustedDistance = Mathf.Max(hit.distance - collisionOffset, minDistanceFromTarget);
            return start + direction * adjustedDistance;
        }

        return desired;
    }

    private void OnDrawGizmos()
    {
        if (target != null)
        {
            Gizmos.color = Color.red;
            Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
            Vector3 direction = rotation * -Vector3.forward;
            Gizmos.DrawRay(target.position, direction * distanceFromTarget);
        }
    }
}