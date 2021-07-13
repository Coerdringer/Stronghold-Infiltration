using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class NewInputSystem : MonoBehaviour
{
    [SerializeField]
    float speed = 12f;

    CharacterController characterController;

    Vector3 moveVector;
    Vector2 direction;

    Vector3 flyVector;
    Vector3 flyDirection;

    // Start is called before the first frame update
    void Start()
    { 
        characterController = GetComponent<CharacterController>();
    }

    private void FixedUpdate()
    {   
        float x = direction.x;

        float y = flyDirection.y;

        float z = direction.y;
        

        Vector3 move = transform.right * x + transform.forward * z;
        Vector3 fly = transform.up * y;

        characterController.Move(move * speed * Time.fixedDeltaTime);
        characterController.Move(fly * speed * Time.deltaTime);
    }

    public void OnMovementChanged(InputAction.CallbackContext context)
    {
        direction = context.ReadValue<Vector2>();
        moveVector = new Vector3(direction.x, 0, direction.y);
    }

    public void Flying(InputAction.CallbackContext context)
    {
        flyDirection = context.ReadValue<Vector2>();
        flyVector = new Vector3(0, flyDirection.y, 0);
    }
}
