using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMove : MonoBehaviour
{
    public float moveSpeed = 5f; //Karakterin hýzýný ayarlar
    public float lookSpeed = 2f; //Karakterin mouse göre dönme hýzýný ayarlar
    public Transform playerCamera;

    private float rotationX = 0f; //Mousun x eksenindeki haraketine göre

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; //mouseun kitlenmesini ve gizlenmesini saðlýyor
    }

    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal"); //"a" "d" yönler için
        float moveVertical = Input.GetAxis("Vertical");
        Vector3 movement = (transform.right * moveHorizontal + transform.forward * moveVertical).normalized;
        transform.position += movement * moveSpeed * Time.deltaTime;


        float mouseX = Input.GetAxis("Mouse X") * lookSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * lookSpeed;

        transform.Rotate(0, mouseX, 0);

        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -90, 90);
        playerCamera.localEulerAngles = new Vector3(rotationX, 0f, 0f);
    }
}
