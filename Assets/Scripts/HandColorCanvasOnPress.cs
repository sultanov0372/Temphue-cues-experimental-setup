using GLTFast.Schema;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using VRQuestionnaireToolkit;
using Material = UnityEngine.Material;

public class HandColorCanvasOnPress : MonoBehaviour
{
    private bool isColliding = false; // To track if collision is ongoing
    public float collisionTime = 0f;
    public float requiredCollisionTime = 6f;

    public GameObject pointerObject;


    public int questionnaireIndex = 0;
    public TextMeshProUGUI timerText;

    private GameObject _vrQuestionnaireToolkit;
    private GenerateQuestionnaire _generateQuestionnaire;
    private ExportToCSV _exportToCsvScript;

    public List<GameObject> currentlyColliding = new List<GameObject>();



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

        _vrQuestionnaireToolkit = GameObject.FindGameObjectWithTag("VRQuestionnaireToolkit");
        _generateQuestionnaire = _vrQuestionnaireToolkit.GetComponentInChildren<GenerateQuestionnaire>();

        var exportToCsvObj = GameObject.FindGameObjectWithTag("ExportToCSV");
        _exportToCsvScript = exportToCsvObj.GetComponent<ExportToCSV>();

        //_exportToCsvScript.QuestionnaireFinishedEvent.RemoveAllListeners();
        _exportToCsvScript.QuestionnaireFinishedEvent.AddListener(OnQuestionnaireFinished);

        if (timerText != null)
            timerText.gameObject.SetActive(false);

        SetPointerActive(false);

    }

    private void Update()
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
                ShowQuestionnaire();
                isColliding = false;
            }
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

            if (!currentlyColliding.Contains(other.gameObject))
            {
                currentlyColliding.Add(other.gameObject);
            }

            collisionTime = 0f;
            isColliding = true;

            if (timerText != null)
            {
                timerText.gameObject.SetActive(true);
                timerText.text = "0.0s";
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

            if (currentlyColliding.Contains(other.gameObject))
            {
                currentlyColliding.Remove(other.gameObject);
            }

            isColliding = false;
            collisionTime = 0f;

            if (timerText != null)
                timerText.gameObject.SetActive(false);
        }
        Debug.Log("exit collision with: " + other.gameObject.name);
    }

    private void ShowQuestionnaire()
    {
        // Deactivate all first
        foreach (var q in _generateQuestionnaire.Questionnaires)
            q.SetActive(false);

        if (questionnaireIndex < _generateQuestionnaire.Questionnaires.Count)
            _generateQuestionnaire.Questionnaires[questionnaireIndex].SetActive(true);

        if (timerText != null)
            timerText.gameObject.SetActive(false);

        SetPointerActive(true);

    }

    private void OnQuestionnaireFinished()
    {
        Debug.Log("Questionnaire " + questionnaireIndex + " finished.");
        //_exportToCsvScript.Save();

        if (questionnaireIndex < _generateQuestionnaire.Questionnaires.Count)
            _generateQuestionnaire.Questionnaires[questionnaireIndex].SetActive(false);

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

        if (questionnaireIndex < _generateQuestionnaire.Questionnaires.Count)
            _generateQuestionnaire.Questionnaires[questionnaireIndex].SetActive(true);
    }
}
