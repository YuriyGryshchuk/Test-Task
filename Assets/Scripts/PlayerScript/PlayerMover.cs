using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMover : MonoBehaviour
{
    [SerializeField]
    private float _playerSpeed;
    [SerializeField]
    private FixedJoystick _fixedJoystick;

    private Rigidbody _playerRigidbody;

    private void Start()
    {
        _playerRigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        _playerRigidbody.velocity = new Vector3(_fixedJoystick.Horizontal * _playerSpeed,
            _playerRigidbody.velocity.y, _fixedJoystick.Vertical * _playerSpeed);
    }
}
