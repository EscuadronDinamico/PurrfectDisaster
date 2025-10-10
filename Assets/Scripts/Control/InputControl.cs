using UnityEngine;
using UnityEngine.InputSystem;

public class InputControl : MonoBehaviour
{
    public Vector2 MovementVector {  get; private set; }
    private void OnMove(InputValue inputValue)
    {
        MovementVector= inputValue.Get<Vector2>();

    }

}
