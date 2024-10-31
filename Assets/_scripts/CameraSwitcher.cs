using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public Camera mainCamera; // Kameray� burada atay�n
    public GameObject objectA; // 1. GameObject
    public GameObject objectB; // 2. GameObject

    private bool isObjectAActive = true; // Hangi GameObject'in aktif oldu�unu takip et
    public float transitionSpeed = 1.0f; // Ge�i� h�z�

    void Update()
    {
        // Tab tu�una bas�ld���nda ge�i� yap
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isObjectAActive = !isObjectAActive; // Aktif GameObject'i de�i�tir
        }

        // Ge�i� yapacak GameObject'in konumunu hesapla
        Vector3 targetPosition = isObjectAActive ? objectA.transform.position : objectB.transform.position;

        // Kameray� smooth bir �ekilde ge�i� yapacak �ekilde hareket ettir
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetPosition, transitionSpeed * Time.deltaTime);
    }
}
