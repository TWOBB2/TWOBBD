using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMove : MonoBehaviour
{
    public float moveSpeed = 5f; // Karakterin normal h�z�n� ayarlar
    public float sprintSpeed = 10f; // Karakterin ko�ma h�z�n� ayarlar
    public float lookSpeed = 2f; // Karakterin mouse'a g�re d�nme h�z�n� ayarlar
    public float jumpForce = 5f; // Karakterin z�plama kuvvetini ayarlar
    public Transform playerCamera; // Kameray� ekleyece�imiz alan olu�turuyor scriptin �zerine

    private float rotationX = 0f; // Mousun x eksenindeki hareketine g�re
    private Rigidbody rb; // Rigidbody referans�

    private bool canJump = true; // Z�play�p z�plamayaca��n� kontrol eden kod
    private float jumpCooldown = 1f; // Z�plama i�in gereken s�re

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Mouse'un kitlenmesini ve gizlenmesini sa�l�yor
        rb = GetComponent<Rigidbody>(); // Rigidbody bile�enini al
    }

    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal"); // "a" "d" y�nler i�in
        float moveVertical = Input.GetAxis("Vertical");

        // Ko�ma kontrol�
        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : moveSpeed; // E�er sol shift tu�una bas�l�rsa ko�ma h�z�n� kullan

        Vector3 movement = (transform.right * moveHorizontal + transform.forward * moveVertical).normalized;
        transform.position += movement * currentSpeed * Time.deltaTime;

        float mouseX = Input.GetAxis("Mouse X") * lookSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * lookSpeed;

        transform.Rotate(0, mouseX, 0);

        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -90, 90);
        playerCamera.localEulerAngles = new Vector3(rotationX, 0f, 0f);

        // Z�plama kontrol�
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded() && canJump)
        {
            Jump();
        }
    }

    private void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        canJump = false;
        StartCoroutine(JumpCooldown());
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1.1f);
    }

    private IEnumerator JumpCooldown()
    {
        yield return new WaitForSeconds(jumpCooldown);
        canJump = true;
    }
}
