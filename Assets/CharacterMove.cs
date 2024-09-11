using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMove : MonoBehaviour
{
    public float moveSpeed = 5f; //Karakterin h�z�n� ayarlar
    public float lookSpeed = 2f; //Karakterin mouse g�re d�nme h�z�n� ayarlar
    public Transform playerCamera;

    private float rotationX = 0f; //Mousun x eksenindeki haraketine g�re

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; //mouseun kitlenmesini ve gizlenmesini sa�l�yor
    }

    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal"); //"a" "d" y�nler i�in
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
