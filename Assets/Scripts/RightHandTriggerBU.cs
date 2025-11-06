using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RightHandTriggerBU : MonoBehaviour
{

    public ParticleSystem particleSystemPrefab; // Assigned particle system prefab 
    private ParticleSystem activeParticleSystem; // Reference to the instantiated particle system
    private bool isColliding = false; // To track if collision is ongoing
    private float collisionTime = 0f;



    // This method will be called when another collider enters the trigger collider
    private void OnTriggerEnter(Collider other)
    {
        // if collides with mirrored avatars' hand
        if (other.gameObject.CompareTag("MirroredAvatar"))
        {
            Debug.Log("Right hand collider entered the trigger with: " + other.gameObject.name);

            if (particleSystemPrefab != null)
            {
                // Instantiate a new particle system at the collider's position
                activeParticleSystem = Instantiate(particleSystemPrefab, other.transform.position, Quaternion.identity);
                activeParticleSystem.Play(); // Play the particle system
            }
            else
            {
                Debug.LogWarning("Particle system prefab is not assigned!");
            }
        }
    }
    /*
    private void OnTriggerStay(Collider other)
    {
        // if collides with mirrored avatars' hand
        if (other.gameObject.CompareTag("MirroredAvatar"))
        {
            //Debug.Log("STAY " + other.gameObject.name);
            collisionTime += Time.deltaTime; // Increment timer by frame time

            if (collisionTime >= 3f) // Check if 3 seconds have passed
            {
                TriggerParticleEvent(other.transform.position); // Trigger particle system
                isColliding = false; // Stop timing to avoid multiple triggers
            }
        }
    }
    */

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("MirroredAvatar"))
        {
            Debug.Log("Right hand collider exited the trigger with: " + other.gameObject.name);

            if (activeParticleSystem != null)
            {
                Destroy(activeParticleSystem.gameObject); // Destroy the instantiated particle system immediately
                activeParticleSystem = null; // Clear the reference
            }
        }
    }
}
