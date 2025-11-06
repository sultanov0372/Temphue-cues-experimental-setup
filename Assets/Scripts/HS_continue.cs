using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class HS_continue : MonoBehaviour
{
    public Animator animator;
    private float collisionTimer = 0f;
    private bool playerInside = false;
    public float requiredTime = 3f;
    //public TMP_Text timerText;


    void OnTriggerEnter(Collider other)
    {
        Debug.Log("collision with: " +  other.gameObject.name);
        if (other.CompareTag("Player"))
        {
           playerInside = true;
           //animator.SetBool("isHS", true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            collisionTimer = 0f;
            animator.SetBool("isHS", false); // Reset if player leaves early
        }
    }

    void Update()
    {
        if (playerInside)
        {
            collisionTimer += Time.deltaTime;

            if (collisionTimer >= requiredTime)
            {
                animator.SetBool("isHS", true);
            }
        }
    }
}
