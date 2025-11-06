using GLTFast.Schema;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Material = UnityEngine.Material;

public class RightHandTrigger : MonoBehaviour
{

    public ParticleSystem particleSystemSocial; // Assigned particle system prefab social
    public ParticleSystem particleSystemHot; // Assigned particle system prefab hot

    private ParticleSystem activeParticleSystem; // Reference to the instantiated particle system
    private bool isColliding = false; // To track if collision is ongoing
    private float collisionTime = 0f;

    // for shader
    [SerializeField] private Material material;
    public float fadeToGraySpeed = 0.25f; // Speed of fading
    public float fadeToColorSpeed = 2f; // Speed of fading
    private float targetGrayAmount = 0f;
    private Coroutine fadeCoroutine;

    void Start()
    {
        if (material == null)
            material = GetComponent<Renderer>().sharedMaterial;
    }

    // This method will be called when another collider enters the trigger collider
    private void OnTriggerEnter(Collider other)
    {
        // if collides with mirrored avatars' hand
        if (other.gameObject.CompareTag("MirroredAvatar"))
        {
            Debug.Log("Right hand collider entered the trigger with: " + other.gameObject.name);

            if (particleSystemSocial != null)
            {
                // Instantiate a new particle system at the collider's position
                activeParticleSystem = Instantiate(particleSystemSocial, other.transform.position, Quaternion.identity);
                activeParticleSystem.Play(); // Play the particle system
            }
            else
            {
                Debug.LogWarning("Particle system prefab is not assigned!");
            }

            // Start fading to grayscale
            targetGrayAmount = 1f;
            StartFading(fadeToGraySpeed);
        }

        if (other.gameObject.CompareTag("Torch"))
        {
            Debug.Log("Right hand collider entered the trigger with: " + other.gameObject.name);

            if (particleSystemHot != null)
            {
                // Instantiate a new particle system at the collider's position
                activeParticleSystem = Instantiate(particleSystemHot, other.transform.position, Quaternion.identity);
                activeParticleSystem.Play(); // Play the particle system
            }
            else
            {
                Debug.LogWarning("Particle system prefab is not assigned!");
            }

            // Start fading to grayscale
            targetGrayAmount = 1f;
            StartFading(fadeToGraySpeed);
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

            // Start fading back to color
            targetGrayAmount = 0f;
            StartFading(fadeToColorSpeed);
        }

        if (other.gameObject.CompareTag("Torch"))
        {
            Debug.Log("Right hand collider exited the trigger with: " + other.gameObject.name);

            if (activeParticleSystem != null)
            {
                Destroy(activeParticleSystem.gameObject); // Destroy the instantiated particle system immediately
                activeParticleSystem = null; // Clear the reference
            }

            // Start fading back to color
            targetGrayAmount = 0f;
            StartFading(fadeToColorSpeed);
        }
    }
    void StartFading(float speed)
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeGrayEffect(speed));
    }
    IEnumerator FadeGrayEffect(float speed)
    {
        float currentGray = material.GetFloat("_GrayAmount");

        while (!Mathf.Approximately(currentGray, targetGrayAmount))
        {
            currentGray = Mathf.MoveTowards(currentGray, targetGrayAmount, speed * Time.deltaTime);
            material.SetFloat("_GrayAmount", currentGray);
            yield return null;
        }
    }
}
