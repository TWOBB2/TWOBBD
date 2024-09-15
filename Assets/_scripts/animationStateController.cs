using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationStateController : MonoBehaviour
{
    Animator animator;
    int isWalkingHash;
    int isRunningHash;
    int isWalkingBackHash;

    void Start()
    {
        animator = GetComponent<Animator>();
        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
        isWalkingBackHash = Animator.StringToHash("isWalkingBack");
    }


    void Update()
    {
        bool isWalkingBack = animator.GetBool(isWalkingBackHash);
        bool isRunning = animator.GetBool(isRunningHash);
        bool isWalking = animator.GetBool(isWalkingHash);
        bool forwardPressed = Input.GetKey("w");
        bool runPressed = Input.GetKey("left shift");
        bool backPressed = Input.GetKey("s");


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

        // eðer oyuncu KOÞUYORSA ve koşmayı yada yürümeyi býrakýrsa
        if (isRunning && (!forwardPressed || !runPressed))
        {
            // isRunning Boolean devredýþý kalır
            animator.SetBool(isRunningHash, false);
        }

        //eğer karakter geriye gitmiyorsa ve S tuşuna basarsa
        if (!isWalkingBack && backPressed)
        {
            // geriye gitme animasyonu aktifleşir
            animator.SetBool(isWalkingBackHash, true);
        }

        // Eğer Karakter geriye yürüyorsa ve yürümeyi bırakıyorsa
        if (isWalkingBack && !backPressed)
        {
            // Idle pozisyonuna dönmüş olur
            animator.SetBool(isWalkingBackHash, false);
        }

        // Eğer karakter geri yürüyorsa ve W tuşuna basarsa
        if (isWalkingBack && forwardPressed)
        {
            // ileri yürüme animasyonu devreye girer
            animator.SetBool(isWalkingHash, true);
            
        }

        
        
        

    }
}
