using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public Camera mainCamera; // Kamerayý burada atayýn
    public GameObject objectA; // 1. GameObject
    public GameObject objectB; // 2. GameObject

    private bool isObjectAActive = true; // Hangi GameObject'in aktif olduðunu takip et
    public float transitionSpeed = 1.0f; // Geçiþ hýzý

    void Update()
    {
        // Tab tuþuna basýldýðýnda geçiþ yap
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isObjectAActive = !isObjectAActive; // Aktif GameObject'i deðiþtir
        }

        // Geçiþ yapacak GameObject'in konumunu hesapla
        Vector3 targetPosition = isObjectAActive ? objectA.transform.position : objectB.transform.position;

        // Kamerayý smooth bir þekilde geçiþ yapacak þekilde hareket ettir
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetPosition, transitionSpeed * Time.deltaTime);
    }
}
