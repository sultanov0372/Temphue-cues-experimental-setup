using GLTFast.Schema;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Material = UnityEngine.Material;
using TMPro;

public class HandChangeColorTrigger1 : MonoBehaviour
{
    private bool isColliding = false; // To track if collision is ongoing
    public float collisionTime = 0f;

    public TMP_Text timerText;

    public float requiredCollisionTime = 10f;


    // public Color collisionColor = Color.red; // The color to apply on collision
    private static readonly int ColorOverrideID = Shader.PropertyToID("_ColorOverride");

    private void Start()
    {
        Renderer handRenderer = GetComponentInChildren<Renderer>();
        if (handRenderer != null)
        {
            Debug.Log("Forcing material assignment...");
            handRenderer.material = new Material(handRenderer.material);  // Create a new instance
        }
    }

    // This method will be called when another collider enters the trigger collider
    private void OnTriggerEnter(Collider other)
    {
        // if collides with mirrored avatars' hand
       
            Debug.Log("collision with: " + other.gameObject.name);

        if (other.CompareTag("Player")) // Ensure this only affects hands
        {
            var lerper = other.GetComponentInParent<HandColorLerp>();
            var colorSource = GetComponent<ColorOnTouch>();

            if (lerper != null && colorSource != null)
            {
                lerper.FadeToColor(colorSource.touchColor);
            }
            collisionTime = 0f;
            isColliding = true;

            if (timerText != null)
            {
                timerText.gameObject.SetActive(true); // <-- Turn timer back on when new collision starts
                timerText.text = "0.0s"; // Reset timer display
            }
        }

    }


    private void OnTriggerExit(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            var lerper = other.GetComponentInParent<HandColorLerp>();
            if (lerper != null)
                lerper.ResetColor();

            isColliding = false;
            collisionTime = 0f;

            if (timerText != null)
                timerText.gameObject.SetActive(false);
        }
        Debug.Log("exit collision with: " + other.gameObject.name);

           
    }

    void Update()
    {
        if (isColliding)
        {
            collisionTime += Time.deltaTime;

            if (timerText != null)
            {
                timerText.text = collisionTime.ToString("F1") + "s";
            }

            if (collisionTime >= requiredCollisionTime)
            {
                isColliding = false;
            }
        }
    }
}
