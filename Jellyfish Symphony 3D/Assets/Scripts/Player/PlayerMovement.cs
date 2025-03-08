using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float sprintMultiplier = 1.5f;
    [SerializeField] private float rotationSpeed = 10f;

    [Header("References")]
    [SerializeField] private Transform cameraTransform;

    [Header("Canvas References")]
    [SerializeField] private Canvas mainMenu;
    [SerializeField] private Canvas pauseMenu;
    [SerializeField] private Canvas saveLoad;
    [SerializeField] private Canvas inventory;

    private Vector3 moveDirection;
    private bool canMove;

    private void Update()
    {
        UpdateCanMove();
        if (!canMove) return;

        HandleMovement();
        HandleRotation();
    }

    private void UpdateCanMove()
    {
        canMove = !(mainMenu.enabled || pauseMenu.enabled || saveLoad.enabled || inventory.enabled);
    }

    private void HandleMovement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 cameraForward = Vector3.ProjectOnPlane(cameraTransform.forward, Vector3.up).normalized;
        Vector3 cameraRight = Vector3.ProjectOnPlane(cameraTransform.right, Vector3.up).normalized;

        moveDirection = (cameraForward * vertical + cameraRight * horizontal).normalized;
        
        float currentSpeed = moveSpeed;
        if (Input.GetKey(KeyCode.LeftShift) && vertical > 0)
        {
            currentSpeed *= sprintMultiplier;
        }

        transform.Translate(moveDirection * currentSpeed * Time.deltaTime, Space.World);
    }

    private void HandleRotation()
    {
        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}