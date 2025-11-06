using UnityEngine;
using UnityEngine.SceneManagement;

public class Loader : MonoBehaviour
{
    [SerializeField] private string[] sceneSequence;
    private int currentSceneIndex;

    void Start()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;

        // Find the current scene's index in the sequence
        currentSceneIndex = System.Array.IndexOf(sceneSequence, currentSceneName);
        if (currentSceneIndex == -1)
        {
            Debug.LogError("Current scene is not in the sequence.");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            LoadNextScene();
        }

        if (Input.GetKeyDown(KeyCode.O)) // Reload current scene
        {
            ReloadCurrentScene();
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            LoadPreviousScene();
        }
    }

    private void LoadPreviousScene()
    {
        if (currentSceneIndex - 1 >= 0)
        {
            SceneManager.LoadScene(sceneSequence[currentSceneIndex - 1]);
        }
        else
        {
            Debug.Log("Already at the beginning of the sequence.");
        }
    }

    private void ReloadCurrentScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }
    private void LoadNextScene()
    {
        if (currentSceneIndex + 1 < sceneSequence.Length)
        {
            string nextSceneName = sceneSequence[currentSceneIndex + 1];
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.Log("End of sequence reached.");
        }
    }
}
