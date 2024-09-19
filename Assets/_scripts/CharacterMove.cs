using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class CharacterMove : MonoBehaviour
{
    [SerializeField] bool _canMove = false;
    [SerializeField] float _speed = 3f;
    [SerializeField] float _runSpeed = 6f;
    [SerializeField] float _crouchSpeed = 1.5f;
    [SerializeField] CharacterController _characterController;
    [SerializeField] Animator _animator;
    public Transform playerCamera;
    public Transform cameraHolder;
    public float _CameraSpeed = 5f;
    public float _MouseSensitivity = 10f;
    float xRotation = 0f;
    public Vector3 velocity;
    public float gravity = -9.81f;
    GameInputActions _input;
    Vector3 _movementDirection;
    bool _isRunning = false;
    bool _isCrouch = false;
    Vector3 _originalCameraPosition;
    float _originalHeight;
    public float crouchHeight = 1f;
    public float crouchCameraOffset = 0.5f;

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
        _originalHeight = _characterController.height; // Orijinal y�ksekli�i kaydet
        _originalCameraPosition = cameraHolder.localPosition; // Kameran�n ba�lang�� pozisyonunu kaydet
    }

    void OnEnable()
    {
        _input.Player.Move.performed += HandleOnMovement;
        _input.Player.Move.canceled += HandleOnMovement;
        _input.Player.Run.performed += HandleOnRun; // Shift tu�una bas�ld���nda
        _input.Player.Run.canceled += HandleOnRun;  // Shift tu�u b�rak�ld���nda
        _input.Player.Crouch.performed += HandleOnCrouch; // CTRL tu�una bas�ld���nda
        _input.Player.Crouch.canceled += HandleOnCrouch;  // CTRL tu�u b�rak�ld���nda

        _input.Enable();
    }

    void OnDisable()
    {
        _input.Player.Move.performed -= HandleOnMovement;
        _input.Player.Move.canceled -= HandleOnMovement;
        _input.Player.Run.performed -= HandleOnRun;
        _input.Player.Run.canceled -= HandleOnRun;
        _input.Player.Crouch.performed -= HandleOnCrouch; // CTRL tu�una bas�ld���nda
        _input.Player.Crouch.canceled -= HandleOnCrouch;  // CTRL tu�u b�rak�ld���nda

        _input.Disable();
    }

    void FixedUpdate()
    {
 if (!_canMove) return;

        // Hangi h�z�n kullan�laca��n� belirle (normal, ko�ma veya e�ilme)
        float currentSpeed = _isCrouch ? _crouchSpeed : (_isRunning ? _runSpeed : _speed);

        // Hareket y�n�n� kameraya g�re hesapla (yaln�zca yatay d�zlemde, Y ekseni s�f�rlanm��)
        Vector3 forward = playerCamera.transform.forward;
        Vector3 right = playerCamera.transform.right;

        // Y eksenini s�f�rl�yoruz, b�ylece sadece yatay (X-Z) hareket sa�lan�yor
        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        // Kamera y�n�ne g�re hareket vekt�r�n� olu�tur
        Vector3 moveDirection = forward * _movementDirection.z + right * _movementDirection.x;

        // Karakteri hareket ettir
        _characterController.Move(Time.deltaTime * currentSpeed * moveDirection);

        // Yer�ekimi ve d���� ekle
        if (!_characterController.isGrounded)
        {
            velocity.y += gravity * Time.deltaTime;
        }
        else
        {
            velocity.y = -2f; // Hafif bir a�a�� kuvvet uyguluyoruz ki yere sabit kals�n
        }
        _characterController.Move(velocity * Time.deltaTime);
    }

    void LateUpdate()
    {
        _animator.SetFloat("speed", _movementDirection.magnitude);

        // `isRunning` parametresini animator'a g�nderiyoruz
        _animator.SetBool("isRunning", _isRunning);
        _animator.SetBool("isCrouch", _isCrouch);

        // Blend Tree i�in X ve Y h�z de�erleri
        _animator.SetFloat("Velocity X", _movementDirection.x, 0.1f, Time.deltaTime);
        _animator.SetFloat("Velocity Y", _movementDirection.z, 0.1f, Time.deltaTime);

        MouseLook();

        // Kamera pozisyonunu e�ilme durumuna g�re ayarla
        UpdateCameraPosition();
    }

    private void MouseLook()
    {
        float mouseX = _input.Player.Look.ReadValue<Vector2>().x * _MouseSensitivity * Time.deltaTime;
        float mouseY = _input.Player.Look.ReadValue<Vector2>().y * _MouseSensitivity * Time.deltaTime;

        // X ekseni (yukar�-a�a�� bak��) s�n�rlamas�
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -89f, 89f);

        // Kameray� d�nd�r
        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX); // Karakterin sa�a sola d�nmesini sa�l�yor
    }

    private void HandleOnMovement(InputAction.CallbackContext context)
    {
        Vector2 direction = context.ReadValue<Vector2>();
        _movementDirection = new Vector3(direction.x, y: 0f, z: direction.y);
    }

    #region KON�MA 

    private void HandleOnRun(InputAction.CallbackContext context)
    {
        // E�er e�iliyorsa ko�may� engelle
        if (_isCrouch)
        {
            _isRunning = false;
            return;
        }

        _isRunning = context.ReadValueAsButton();

    }

    #endregion

    private void HandleOnCrouch(InputAction.CallbackContext context)
    {
        _isCrouch = context.ReadValueAsButton();


        // E�er ko�uyorsa e�ilmeyi engelle
        if (_isRunning)
        {
            _isCrouch = false;
            return;
        }

        // E�ilme durumuna g�re karakterin y�ksekli�ini ve merkezini ayarla
        if (_isCrouch)
        {
            _characterController.height = crouchHeight; // Karakterin y�ksekli�ini d���r
            _characterController.center = new Vector3(0, crouchHeight / 2f, 0); // Y�ksekli�e g�re merkezini ayarla
        }
        else
        {
            _characterController.height = _originalHeight; // Eski y�ksekli�e geri d�n
            _characterController.center = new Vector3(0, _originalHeight / 2f, 0); // Merkezini eski haline getir
        }
    }

    private void UpdateCameraPosition()
    {
        if (_isCrouch)
        {
            // E�ildi�inde kamera a�a�� ve ileri gider
            cameraHolder.localPosition = _originalCameraPosition - new Vector3(0, crouchCameraOffset, -0.21f); // Z ekseninde de ileriye kayd�r�yoruz
        }
        else
        {
            cameraHolder.localPosition = _originalCameraPosition; // Eski pozisyona geri d�n
        }
    }
}
