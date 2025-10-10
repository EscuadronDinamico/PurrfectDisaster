using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class Player : MonoBehaviour
{
    private Rigidbody rb;
    private float upForce = 250f, force=10f;
    private PlayerInput playerInput;
    private Vector2 input;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb= GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {
        input= playerInput.actions["Move"].ReadValue<Vector2>();
        Debug.Log(input);

    }
    private void FixedUpdate()
    {
        rb.AddForce(new Vector3(input.x, 0f, input.y) * force);
    }
    public void Jump(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            rb.AddForce(Vector3.up * upForce);
        }
        Debug.Log(callbackContext.phase);
    }
}
