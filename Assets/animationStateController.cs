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


        // eğer oyuncu W basarsa
        if (!isWalking && forwardPressed)
        {
            // isWalking Boolean aktif hale geliyor (kutucuk tikli)
            animator.SetBool(isWalkingHash, true);
        }
        // Eğer oyuncu W basmazsa 
        if (isWalking && !forwardPressed)
        {   
            // isWalking Boolean kapanır ve tekrar idle animasyonuna geçer
            animator.SetBool(isWalkingHash, false); 
        }

        // eğer oyuncu yürüyorsa ve koşmuyorsa ve L Shifte basarsa
        if (!isRunning && (forwardPressed && runPressed))
        {
            // isRunning Boolean aktif hale gelir
            animator.SetBool (isRunningHash, true);
        }

        // eğer oyuncu KOŞUYORSA ve koşmayı yada yürümeyi bırakırsa
        if (isRunning && (!forwardPressed || !runPressed))
        {
            // isRunning Boolean devredışı kalır
            animator.SetBool(isRunningHash, false);
        }


    }
}
