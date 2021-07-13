using UnityEngine;
using UnityEngine.InputSystem;

public class MouseController : MonoBehaviour
{
    [SerializeField]
    Transform playerBody;
    
    [SerializeField]
    float mouseSensitivity = 30f;

    float xRotation = 0f;

    Vector2 delta;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate()
    {
        float mouseX = delta.x * mouseSensitivity * Time.deltaTime;
        float mouseY = delta.y * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.transform.Rotate(Vector2.up * mouseX);
    }

    public void MouseControl(InputAction.CallbackContext context)
    {
        delta = context.ReadValue<Vector2>();
    }
}
