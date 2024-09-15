using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public CinemachineVirtualCamera VCAMFACE; // �lk sanal kamera
    public CinemachineVirtualCamera VCAM3RDP; // �kinci sanal kamera

    private void Start()
    {
        // �lk ba�ta, ilk kameray� aktif yap
        ActivateCamera(VCAMFACE);
    }

    private void Update()
    {
        // �rnek: Bo�luk tu�una bas�ld���nda kameralar aras�nda ge�i� yap
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
        // Ge�i� yapmak istedi�iniz kameran�n �nceli�ini y�ksek yap�n
        cameraToActivate.Priority = 10;

        // Di�er kameralar�n �nceli�ini d���k yap�n
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
