using System.Collections.Generic;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Rigidbody))]
public class CarMovement : MonoBehaviour
{
    private InputManager _inputManager;

    private Vector2 _movementDirection;

    private new Rigidbody _rb;
    private float _currentSpeed;

    private bool _isStop = false;

    [Header("Speed Stats")]
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

    [Header("Visual")]
    [SerializeField]
    private Transform _visual;

    [SerializeField]
    private List<Transform> _rotationWheels;

    [SerializeField]
    private float _visualRotationMaxDegree;

    [SerializeField]
    private float _visualRotationSpeed;

    [SerializeField]
    private float _visualResetRotationSpeed;

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

    private void FixedUpdate()
    {
        if (_isStop)
        {
            _rb.velocity = Vector3.zero;
            return;
        }

        _currentSpeed = _rb.velocity.magnitude;

        _movementDirection = MovementInput();

        Move();
    }

    private void Move()
    {
        if (_movementDirection.x != 0)
        {
            if (_movementDirection.x < 0)
                TurnLeft();
            else if (_movementDirection.x > 0)
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