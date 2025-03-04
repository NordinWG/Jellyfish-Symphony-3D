using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	[Header("Base setup")]
	public int speed = 4;
	public int sprintSpeed = 25;
	public float jumpSpeed = 8.0f;
	public float gravity = 20.0f;
	public float mouseSensitivity = 1500f;
	public float controllerSensitivity = 1500f;
	private int currentSpeed;
	private float rotationX = 0f;
	private float rotationY = 0f;
	private Vector3 moveDirection = Vector3.zero;
	public float hor;
	public float ver;
	public Canvas mainmenu;
	public Canvas pauseMenu;
	public Canvas saveLoad;
	public Canvas inventory;
	public Camera playerCamera;
	private CharacterController characterController;

	[Header("Canvas")]


	[SerializeField]
	private float cameraYOffset = 0.4f;
	public bool canMove;

	void Start()
	{

		speed = 4;
		sprintSpeed = 25;

		characterController = GetComponent<CharacterController>();
		
		// Assign the main camera if it's not assigned
		if (playerCamera == null)
		{
			playerCamera = Camera.main;

			if (playerCamera == null) return;
		}

		// Set the camera's parent to the player object, if not already done in the Inspector
		if (playerCamera.transform.parent != transform)
		{
			playerCamera.transform.SetParent(transform);
			playerCamera.transform.localPosition = new Vector3(0, cameraYOffset, 0); // Adjust if necessary
		}

		currentSpeed = speed;
	}

	void Update()
	{
		print(mainmenu.enabled || pauseMenu.enabled || saveLoad.enabled || inventory.enabled);

		if (playerCamera != null && !playerCamera.gameObject.activeInHierarchy)
		{
			playerCamera.gameObject.SetActive(true);
		}

        if(mainmenu.enabled || pauseMenu.enabled || saveLoad.enabled || inventory.enabled)
		{
			canMove = false;
		}
		if(!mainmenu.enabled && !pauseMenu.enabled && !saveLoad.enabled && !inventory.enabled)
		{
			canMove = true;
			Cursor.visible = false;
        	Cursor.lockState = CursorLockMode.Locked;
		}
        
		Movement();
		RotatePlayer();
	}

	void Movement()
	{
		if (!canMove) return;

		hor = Input.GetAxis("Horizontal");
		ver = Input.GetAxis("Vertical");

		currentSpeed = Input.GetKey(KeyCode.LeftControl) ? sprintSpeed : speed;

		Vector3 forward = transform.TransformDirection(Vector3.forward);
		Vector3 right = transform.TransformDirection(Vector3.right);
		float curSpeedX = currentSpeed * ver;
		float curSpeedY = currentSpeed * hor;
		float movementDirectionY = moveDirection.y;

		moveDirection = (forward * curSpeedX) + (right * curSpeedY);

		characterController.Move(moveDirection * Time.deltaTime);
	}

	void RotatePlayer()
    {
        if (!canMove) return;

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -90f, 90f);
        rotationY += mouseX;

        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
		transform.localRotation = Quaternion.Euler(0, rotationY, 0);
    }
}