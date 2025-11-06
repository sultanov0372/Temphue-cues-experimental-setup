using GLTFast.Schema;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using VRQuestionnaireToolkit;
using Material = UnityEngine.Material;
using TMPro;


public class ParticleTriggerCanvas : MonoBehaviour
{

    public ParticleSystem leftHandPrefab;
    public ParticleSystem rightHandPrefab;
    public int leftHandLayer = 12;
    public int rightHandLayer = 13;

    private int collidingObjectsCount = 0;
    private int collidingHandsCount = 0;
    public float collisionTime = 0f;
    public float requiredCollisionTime = 10f;

    private GameObject _vrQuestionnaireToolkit;
    private GenerateQuestionnaire _generateQuestionnaire;
    private GameObject _exportToCSV;


    private Dictionary<Collider, ParticleSystem> activeParticles = new Dictionary<Collider, ParticleSystem>();

    public GameObject questionnaireCanvas; 
    public GameObject pointerObject;

    private int currentQuestionnaireIndex = 0; // Track which questionnaire is active

    private ExportToCSV _exportToCsvScript;
    private GameObject _exportToCsv;

    public int questionnaireIndex = 0; // <-- Public index you set in Inspector!
    public TMP_Text timerText;

    private List<GameObject> currentlyColliding = new List<GameObject>();



    void Start()
    {
        _vrQuestionnaireToolkit = GameObject.FindGameObjectWithTag("VRQuestionnaireToolkit");
        _generateQuestionnaire = _vrQuestionnaireToolkit.GetComponentInChildren<GenerateQuestionnaire>();
        SetPointerActive(false);

        _exportToCsv = GameObject.FindGameObjectWithTag("ExportToCSV");
        _exportToCsvScript = _exportToCsv.GetComponent<ExportToCSV>();
        //_exportToCsvScript.QuestionnaireFinishedEvent.AddListener(OnQuestionnaireFinished);

        _exportToCsvScript.QuestionnaireFinishedEvent.RemoveAllListeners();
        _exportToCsvScript.QuestionnaireFinishedEvent.AddListener(OnQuestionnaireFinished);
    }


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


                    if (!currentlyColliding.Contains(other.gameObject))
                    {
                        currentlyColliding.Add(other.gameObject);
                    }

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
                    if (currentlyColliding.Contains(other.gameObject))
                    {
                        currentlyColliding.Remove(other.gameObject);
                    }
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
                ShowQuestionnaire();
                collidingHandsCount = 0;
            }
        }
    }

    void ShowQuestionnaire()
    {
        // Hide all first
        foreach (var questionnaire in _generateQuestionnaire.Questionnaires)
        {
            questionnaire.SetActive(false);
        }

        if (questionnaireIndex >= 0 && questionnaireIndex < _generateQuestionnaire.Questionnaires.Count)
        {
            _generateQuestionnaire.Questionnaires[questionnaireIndex].SetActive(true);
        }
        else
        {
            Debug.LogWarning("Questionnaire index out of range!");
        }

        SetPointerActive(true);

        if (timerText != null)
        {
            timerText.gameObject.SetActive(false);
        }

    }

    void OnQuestionnaireFinished()
    {
        Debug.Log("Questionnaire submitted!");
       
        _generateQuestionnaire.Questionnaires[questionnaireIndex].SetActive(false);

        // Hide pointer again
        SetPointerActive(false);

    }

    void SetPointerActive(bool isActive)
    {
        if (pointerObject != null)
        {
            pointerObject.SetActive(isActive);
        }
    }

    public GameObject GetFirstCollision()
    {
        return currentlyColliding.Count > 0 ? currentlyColliding[0] : null;
    }

    public void ActivateQuestionnaireBasedOnCollision()
    {
        GameObject touchedObject = GetFirstCollision();

        if (touchedObject == null)
        {
            Debug.Log("No object is being touched.");
            return;
        }

        Debug.Log("Touched: " + touchedObject.name);

        ShowQuestionnaire();
    }
}
