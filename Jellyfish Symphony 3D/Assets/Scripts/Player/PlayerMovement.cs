using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed;
    public float rotationSpeed;

    [Header("References")]
    public Transform cameraTransform;

    [Header("Canvas References")]
    public Canvas mainMenu;
    public Canvas pauseMenu;
    public Canvas saveLoad;
    public Canvas inventory;
    public Canvas endCutscene;

    private Vector3 moveDirection;
    private bool canMove;
    public Animator animator;

    public void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        UpdateCanMove();
        if (!canMove) return;

        HandleMovement();
        HandleRotation();
    }

    private void UpdateCanMove()
    {
        canMove = !(mainMenu.enabled || pauseMenu.enabled || saveLoad.enabled || inventory.enabled || endCutscene.enabled);
    }

    public void HandleMovement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 cameraForward = Vector3.ProjectOnPlane(cameraTransform.forward, Vector3.up).normalized;
        Vector3 cameraRight = Vector3.ProjectOnPlane(cameraTransform.right, Vector3.up).normalized;

        moveDirection = (cameraForward * vertical + cameraRight * horizontal).normalized;
        
        float currentSpeed = moveSpeed;

        transform.Translate(moveDirection * currentSpeed * Time.deltaTime, Space.World);

        float moveInput = Input.GetAxisRaw("Horizontal") + Input.GetAxisRaw("Vertical");
        animator.SetFloat("speed", Mathf.Abs(moveInput));
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