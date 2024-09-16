using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMove : MonoBehaviour
{
    [SerializeField] bool _canMove = false;
    [SerializeField] float _speed = 3f;
    [SerializeField] float _runSpeed = 6f;
    [SerializeField] CharacterController _characterController;
    [SerializeField] Animator _animator;
    public Transform playerCamera;

    public Vector3 velocity;


    public float gravity = -9.81f;

    GameInputActions _input;
    Vector3 _movementDirection; // Karakterin hangi y�ne hareket edece�ini temsil eden vekt�r
    bool _isRunning = false;

    private void Update()
    {


        // Yer�ekimi uygulamas�
        velocity.y += gravity * Time.deltaTime;
        _characterController.Move(velocity * Time.deltaTime);
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void OnValidate()
    {
        Cursor.lockState = CursorLockMode.Locked;

        if (_characterController == null)
        {
            _characterController = GetComponent<CharacterController>();
        } 

       if (_animator == null)
        {
            _animator = GetComponent<Animator>();
        }
    }

    void Awake()
    {
        _input = new GameInputActions();
    }

    void OnEnable()
    {
        _input.Player.Move.performed += HandleOnMovement;
        _input.Player.Move.canceled += HandleOnMovement;
        _input.Player.Run.performed += HandleOnRun; // Shift tu�una bas�ld���nda
        _input.Player.Run.canceled += HandleOnRun;  // Shift tu�u b�rak�ld���nda

        _input.Enable();
    }

    void OnDisable()
    {
        _input.Player.Move.performed -= HandleOnMovement;
        _input.Player.Move.canceled -= HandleOnMovement;
        _input.Player.Run.performed -= HandleOnRun;
        _input.Player.Run.canceled -= HandleOnRun;

        _input.Disable();
    }

    void FixedUpdate()
    {
        if (!_canMove) return;

        // E�er ko�ma tu�una bas�lm��sa ko�ma h�z�n�, bas�lmam��sa y�r�me h�z�n� kullan
        float currentSpeed = _isRunning ? _runSpeed : _speed;
        _characterController.Move(Time.deltaTime * currentSpeed * _movementDirection);
    }

    void LateUpdate()
    {
        _animator.SetFloat("speed", _movementDirection.magnitude);

        // `isRunning` parametresini animator'a g�nderiyoruz
        _animator.SetBool("isRunning", _isRunning);

        // Blend Tree i�in X ve Y h�z de�erleri
        _animator.SetFloat("Velocity X", _movementDirection.x, 0.1f, Time.deltaTime);
        _animator.SetFloat("Velocity Y", _movementDirection.z, 0.1f, Time.deltaTime);
    }

    private void HandleOnMovement(InputAction.CallbackContext context)
    {
        Vector2 direction = context.ReadValue<Vector2>();
        _movementDirection = new Vector3(direction.x, y: 0f, z: direction.y);
    }

    private void HandleOnRun(InputAction.CallbackContext context)
    {
        _isRunning = context.ReadValueAsButton();
    }

}
