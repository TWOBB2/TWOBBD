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
    [SerializeField] CharacterController _characterController;
    [SerializeField] Animator _animator;
    public Transform playerCamera;

    public float _CameraSpeed = 5f;
    public float _MouseSensitivity = 10f;
    float xRotation = 0f;


    public Vector3 velocity;

    public float gravity = -9.81f;

    GameInputActions _input;
    Vector3 _movementDirection; // Karakterin hangi yöne hareket edeceðini temsil eden vektör
    bool _isRunning = false;

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
        _input.Player.Run.performed += HandleOnRun; // Shift tuþuna basýldýðýnda
        _input.Player.Run.canceled += HandleOnRun;  // Shift tuþu býrakýldýðýnda

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

        // Eðer koþma tuþuna basýlmýþsa koþma hýzýný, basýlmamýþsa yürüme hýzýný kullan
        float currentSpeed = _isRunning ? _runSpeed : _speed;

        // Hareket yönünü kameraya göre hesapla (yalnýzca yatay düzlemde, Y ekseni sýfýrlanmýþ)
        Vector3 forward = playerCamera.transform.forward;
        Vector3 right = playerCamera.transform.right;

        // Y eksenini sýfýrlýyoruz, böylece sadece yatay (X-Z) hareket saðlanýyor
        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        // Kamera yönüne göre hareket vektörünü oluþtur
        Vector3 moveDirection = forward * _movementDirection.z + right * _movementDirection.x;

        // Karakteri hareket ettir
        _characterController.Move(Time.deltaTime * currentSpeed * moveDirection);

        // Yerçekimi ve düþüþ ekle
        if (!_characterController.isGrounded)
        {
            velocity.y += gravity * Time.deltaTime;
        }
        else
        {
            velocity.y = -2f; // Hafif bir aþaðý kuvvet uyguluyoruz ki yere sabit kalsýn
        }
        _characterController.Move(velocity * Time.deltaTime);
    }

    void LateUpdate()
    {
        _animator.SetFloat("speed", _movementDirection.magnitude);

        // `isRunning` parametresini animator'a gönderiyoruz
        _animator.SetBool("isRunning", _isRunning);

        // Blend Tree için X ve Y hýz deðerleri
        _animator.SetFloat("Velocity X", _movementDirection.x, 0.1f, Time.deltaTime);
        _animator.SetFloat("Velocity Y", _movementDirection.z, 0.1f, Time.deltaTime);

        MouseLook();
    }

    private void MouseLook()
    {
        float mouseX = _input.Player.Look.ReadValue<Vector2>().x * _MouseSensitivity * Time.deltaTime;
        float mouseY = _input.Player.Look.ReadValue<Vector2>().y * _MouseSensitivity * Time.deltaTime;

        // X ekseni (yukarý-aþaðý bakýþ) sýnýrlamasý
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -89f, 89f);

        // Kamerayý döndür
        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX); // Karakterin saða sola dönmesini saðlýyor
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
