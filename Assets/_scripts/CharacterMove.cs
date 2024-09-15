using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMove : MonoBehaviour
{
    public float moveSpeed = 5f; // Karakterin normal hýzýný ayarlar
    public float sprintSpeed = 10f; // Karakterin koþma hýzýný ayarlar
    public float lookSpeed = 2f; // Karakterin mouse'a göre dönme hýzýný ayarlar
    public float jumpForce = 5f; // Karakterin zýplama kuvvetini ayarlar
    public Transform playerCamera; // Kamerayý ekleyeceðimiz alan oluþturuyor scriptin üzerine

    private float rotationX = 0f; // Mousun x eksenindeki hareketine göre
    private Rigidbody rb; // Rigidbody referansý

    private bool canJump = true; // Zýplayýp zýplamayacaðýný kontrol eden kod
    private float jumpCooldown = 1f; // Zýplama için gereken süre

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Mouse'un kitlenmesini ve gizlenmesini saðlýyor
        rb = GetComponent<Rigidbody>(); // Rigidbody bileþenini al
    }

    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal"); // "a" "d" yönler için
        float moveVertical = Input.GetAxis("Vertical");

        // Koþma kontrolü
        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : moveSpeed; // Eðer sol shift tuþuna basýlýrsa koþma hýzýný kullan

        Vector3 movement = (transform.right * moveHorizontal + transform.forward * moveVertical).normalized;
        transform.position += movement * currentSpeed * Time.deltaTime;

        float mouseX = Input.GetAxis("Mouse X") * lookSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * lookSpeed;

        transform.Rotate(0, mouseX, 0);

        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -90, 90);
        playerCamera.localEulerAngles = new Vector3(rotationX, 0f, 0f);

        // Zýplama kontrolü
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
