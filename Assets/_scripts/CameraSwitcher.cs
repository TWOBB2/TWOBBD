using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public CinemachineVirtualCamera VCAMFACE; // Ýlk sanal kamera
    public CinemachineVirtualCamera VCAM3RDP; // Ýkinci sanal kamera

    private void Start()
    {
        // Ýlk baþta, ilk kamerayý aktif yap
        ActivateCamera(VCAMFACE);
    }

    private void Update()
    {
        // Örnek: Boþluk tuþuna basýldýðýnda kameralar arasýnda geçiþ yap
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (VCAMFACE.Priority > VCAM3RDP.Priority)
            {
                ActivateCamera(VCAM3RDP);
            }
            else
            {
                ActivateCamera(VCAMFACE);
            }
        }
    }

    private void ActivateCamera(CinemachineVirtualCamera cameraToActivate)
    {
        // Geçiþ yapmak istediðiniz kameranýn önceliðini yüksek yapýn
        cameraToActivate.Priority = 10;

        // Diðer kameralarýn önceliðini düþük yapýn
        if (cameraToActivate != VCAMFACE)
        {
            VCAMFACE.Priority = 5;
        }

        if (cameraToActivate != VCAM3RDP)
        {
            VCAM3RDP.Priority = 5;
        }
    }
}
