using System.Collections.Generic;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Rigidbody))]
public class CarMovement : MonoBehaviour
{
    private InputManager _inputManager;
    private bool _onStart = false;

    private float _horizontalInput;

    private Vector2 _movementDirection;

    private Rigidbody _rb;
    private float _currentSpeed;

    private bool _isStop = true;

    [Header("Speed Stats")]
    [SerializeField]
    private InputHandler _inputHandler;

    [SerializeField]
    private float _maxSpeed = 45;

    [SerializeField]
    private float _minSpeed = 25;

    [SerializeField]
    private float _rotationSpeed = 150;

    [Header("Acceleration Stats")]
    [SerializeField]
    private float _acceleration = 14;

    [SerializeField]
    private float _driftAcceleration = 11;

    [Header("Visual References")]
    [SerializeField]
    private Transform _visual;

    [SerializeField]
    private List<Transform> _rotationWheels;

    [Header("Visual Stats")]
    [SerializeField]
    private float _visualRotationMaxDegree;

    [SerializeField]
    private float _visualRotationSpeed;

    [SerializeField]
    private float _visualResetRotationSpeed;

    [Header("Screen Zones")]
    [SerializeField]
    private float leftZoneThreshold = 0.3f;

    [SerializeField]
    private float rightZoneThreshold = 0.7f;

    [Inject]
    public void Construct(InputManager inputManager)
    {
        _inputManager = inputManager;
    }

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private Vector2 MovementInput()
    {
        return _inputManager._inputActions.Player.Movement.ReadValue<Vector2>();
    }

    private void Update()
    {
        if (Input.anyKeyDown && !_onStart)
        {
            _onStart = true;
            _isStop = false;
        }
    }


    private void FixedUpdate()
    {
        if (_isStop)
        {
            _rb.velocity = Vector3.zero;
            return;
        }

        _currentSpeed = _rb.velocity.magnitude;

        _movementDirection = MovementInput();

        if (_inputHandler._inputType == InputType.PC)
            Move(_movementDirection.x);

        if (_inputHandler._inputType == InputType.Android)
        {
            _horizontalInput = 0f;

            if (Input.touchCount > 0)
            {
                Touch _touch = Input.GetTouch(0);

                float _touchPositionX = _touch.position.x / Screen.width;

                if (_touchPositionX < leftZoneThreshold)
                {
                    _horizontalInput = -1f;
                }
                else if (_touchPositionX > rightZoneThreshold)
                {
                    _horizontalInput = 1f;
                }
            }

            Move(_horizontalInput);
        }
    }

    private void Move(float horizontalAxis)
    {
        if (horizontalAxis != 0)
        {
            if (horizontalAxis < 0)
                TurnLeft();
            else if (horizontalAxis > 0)
                TurnRight();

            _currentSpeed += _driftAcceleration * Time.deltaTime;
        }
        else
        {
            Drive();

            if (_currentSpeed < _maxSpeed)
                _currentSpeed += _acceleration * Time.deltaTime;
        }

        if (_currentSpeed < _minSpeed)
            _currentSpeed = _minSpeed;

        if (_currentSpeed > _maxSpeed)
            _currentSpeed = _maxSpeed;

        _rb.velocity = transform.forward * _currentSpeed;
    }

    public void Crash()
    {
        enabled = false;

        _rb.velocity = Vector3.zero;
    }

    public void Stop()
    {
        enabled = false;

        _rb.velocity = Vector3.zero;
    }

    private void TurnLeft()
    {
        transform.Rotate(Vector3.up * -_rotationSpeed * Time.deltaTime);
        _visual.localRotation = Quaternion.Lerp(_visual.localRotation, Quaternion.Euler(0, -_visualRotationMaxDegree, 0), Time.deltaTime * _visualRotationSpeed);

        foreach (var _wheel in _rotationWheels)
        {
            _wheel.localRotation = Quaternion.Lerp(_wheel.localRotation, Quaternion.Euler(0, -_visualRotationMaxDegree, 0), Time.deltaTime * _visualRotationSpeed);
        }
    }

    private void TurnRight()
    {
        transform.Rotate(Vector3.up * _rotationSpeed * Time.deltaTime);
        _visual.localRotation = Quaternion.Lerp(_visual.localRotation, Quaternion.Euler(0, _visualRotationMaxDegree, 0), Time.deltaTime * _visualRotationSpeed);

        foreach (var _wheel in _rotationWheels)
        {
            _wheel.localRotation = Quaternion.Lerp(_wheel.localRotation, Quaternion.Euler(0, _visualRotationMaxDegree, 0), Time.deltaTime * _visualRotationSpeed);
        }
    }

    private void Drive()
    {
        _visual.localRotation = _visual.localRotation = Quaternion.Lerp(_visual.localRotation, Quaternion.identity, Time.deltaTime * _visualResetRotationSpeed);

        foreach (var _wheel in _rotationWheels)
        {
            _wheel.localRotation = _wheel.localRotation = Quaternion.Lerp(_wheel.localRotation, Quaternion.identity, Time.deltaTime * _visualResetRotationSpeed);
        }
    }
}