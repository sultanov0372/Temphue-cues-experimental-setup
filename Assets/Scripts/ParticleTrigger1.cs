using GLTFast.Schema;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using VRQuestionnaireToolkit;
using Material = UnityEngine.Material;
using TMPro;


public class ParticleTrigger1 : MonoBehaviour
{

    public ParticleSystem leftHandPrefab;
    public ParticleSystem rightHandPrefab;
    public int leftHandLayer = 12;
    public int rightHandLayer = 13;

    private int collidingObjectsCount = 0;
    private int collidingHandsCount = 0;
    private float collisionTime = 0f;
    public float requiredCollisionTime = 10f;

  


    private Dictionary<Collider, ParticleSystem> activeParticles = new Dictionary<Collider, ParticleSystem>();

    public TMP_Text timerText;

    // This method will be called when another collider enters the trigger collider
    private void OnTriggerEnter(Collider other)
    {
        // if collides with mirrored avatars' hand
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("collision with: " + other.gameObject.name);

            HandIdentifier hand = other.GetComponent<HandIdentifier>();

            if (hand != null)
            {
                int layerToUse = (hand.handType == HandType.Left) ? leftHandLayer : rightHandLayer;

                ParticleSystem psPrefab = (hand.handType == HandType.Left)
                ? leftHandPrefab
                : rightHandPrefab;

                // Instantiate particle system and assign correct layer
                ParticleSystem ps = InstantiateParticleSystem(psPrefab, other.transform.position, layerToUse);
                ps.Play();

                activeParticles[other] = ps;
                collidingHandsCount++;

                // If this is the first one colliding, start timer
                if (collidingHandsCount == 1)
                {
                    collisionTime = 0f;

                    if (timerText != null)
                    {
                        timerText.gameObject.SetActive(true); // <-- Turn timer back on when new collision starts
                        timerText.text = "0.0s"; // Reset timer display
                    }
                }
            }

        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("exit collision with: " + other.gameObject.name);

            if (activeParticles.TryGetValue(other, out ParticleSystem ps))
            {
                Destroy(ps.gameObject); // Destroy the corresponding particle system
                activeParticles.Remove(other); // Remove from dictionary

                //collisionTime = 0f;

                collidingHandsCount--;

                // If none are left, stop timer
                if (collidingHandsCount <= 0)
                {
                    collidingHandsCount = 0; // extra safe
                    if (timerText != null)
                    {
                        timerText.gameObject.SetActive(false);
                    }
                }
            }
        }
    }

    private ParticleSystem InstantiateParticleSystem(ParticleSystem effectPrefab, Vector3 position, int layer)
    {
        ParticleSystem newEffect = Instantiate(effectPrefab, position, Quaternion.identity);
        newEffect.gameObject.layer = layer;

        // If the particle system has children, update their layers too
        foreach (Transform child in newEffect.transform)
        {
            child.gameObject.layer = layer;
        }

        return newEffect;
    }

    void Update()
    {
        if (collidingHandsCount > 0)
        {
            collisionTime += Time.deltaTime;

            if (timerText != null)
            {
                timerText.text = collisionTime.ToString("F1") + "s"; // 1 decimal second precision
            }

            if (collisionTime >= requiredCollisionTime)
            {
                collidingHandsCount = 0;
            }
        }
    }
}
