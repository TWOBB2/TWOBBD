using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationStateController : MonoBehaviour
{
    Animator animator;
    int isWalkingHash;
    int isRunningHash;

    void Start()
    {
        animator = GetComponent<Animator>();
        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
    }


    void Update()
    {
        bool isRunning = animator.GetBool(isRunningHash);
        bool isWalking = animator.GetBool(isWalkingHash);
        bool forwardPressed = Input.GetKey("w");
        bool runPressed = Input.GetKey("left shift");


        // e�er oyuncu W basarsa
        if (!isWalking && forwardPressed)
        {
            // isWalking Boolean aktif hale geliyor (kutucuk tikli)
            animator.SetBool(isWalkingHash, true);
        }
        // E�er oyuncu W basmazsa 
        if (isWalking && !forwardPressed)
        {
            // isWalking Boolean kapan�r ve tekrar idle animasyonuna ge�er
            animator.SetBool(isWalkingHash, false);
        }

        // e�er oyuncu y�r�yorsa ve ko�muyorsa ve L Shifte basarsa
        if (!isRunning && (forwardPressed && runPressed))
        {
            // isRunning Boolean aktif hale gelir
            animator.SetBool(isRunningHash, true);
        }

        // e�er oyuncu KO�UYORSA ve ko�may� yada y�r�meyi b�rak�rsa
        if (isRunning && (!forwardPressed || !runPressed))
        {
            // isRunning Boolean devred��� kal�r
            animator.SetBool(isRunningHash, false);
        }


    }
}
