using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandshakeProceed : MonoBehaviour
{
    public Animator animator;
    private float collisionTimer = 0f;
    private bool playerInside = false;
    private float requiredTime = 3f;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
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
