using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private InputControl _playerControl;
    [SerializeField] float _speed;

    private void Awake()
    {
        _playerControl = GetComponent<InputControl>();
    }
    private void Update()
    {
        Vector3 positionChange = new Vector3(
            _playerControl.MovementVector.x,
            0,
            _playerControl.MovementVector.y)
            *Time.deltaTime
            *_speed;

        transform.position += positionChange;
    }
}
