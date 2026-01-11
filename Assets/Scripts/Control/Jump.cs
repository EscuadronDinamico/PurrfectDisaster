using UnityEngine;
using UnityEngine.InputSystem;

public class Jump : MonoBehaviour
{
    private Rigidbody rb;
    [Header("Jump")]
    public float jumpSpeed = 10f;

    private Keyboard keyboard;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        keyboard = Keyboard.current;

    }

    bool IsGrounded() { return Physics.Raycast(transform.position, Vector3.down, 1.1f); }
    // Update is called once per frame
    void Update()
    {
        if (keyboard.spaceKey.wasPressedThisFrame /*&& IsGrounded()*/)
        {
            rb.AddForce(Vector2.up * jumpSpeed, ForceMode.Impulse);
        }
    }
}
