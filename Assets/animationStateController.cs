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


        // eðer oyuncu W basarsa
        if (!isWalking && forwardPressed)
        {
            // isWalking Boolean aktif hale geliyor (kutucuk tikli)
            animator.SetBool(isWalkingHash, true);
        }
        // Eðer oyuncu W basmazsa 
        if (isWalking && !forwardPressed)
        {
            // isWalking Boolean kapanýr ve tekrar idle animasyonuna geçer
            animator.SetBool(isWalkingHash, false);
        }

        // eðer oyuncu yürüyorsa ve koþmuyorsa ve L Shifte basarsa
        if (!isRunning && (forwardPressed && runPressed))
        {
            // isRunning Boolean aktif hale gelir
            animator.SetBool(isRunningHash, true);
        }

        // eðer oyuncu KOÞUYORSA ve koþmayý yada yürümeyi býrakýrsa
        if (isRunning && (!forwardPressed || !runPressed))
        {
            // isRunning Boolean devredýþý kalýr
            animator.SetBool(isRunningHash, false);
        }


    }
}
