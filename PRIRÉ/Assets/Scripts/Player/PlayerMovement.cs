using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 6f;
    public float mouseSensitivity = 2f;
    public float gravity = -9.81f;
    public float jumpForce = 5f;

    private CharacterController controller;
    private Vector3 velocity;
    private float xRotation = 0f;

    public Transform cameraPivot;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;

    }

    // Update is called once per frame
    void Update()
    {
        HandleMouseLook();
        HandleMovement();

    }

    void HandleMovement()
    {
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (controller.isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("jump...");
            velocity.y = jumpForce;
        }   

        velocity.y += gravity * Time.deltaTime;

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right *x + transform.forward *z;
        move *= moveSpeed;

        move.y = velocity.y;

        controller.Move(move * Time.deltaTime);

    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * 100f * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * 100f * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cameraPivot.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

}
