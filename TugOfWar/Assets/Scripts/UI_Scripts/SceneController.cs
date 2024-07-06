using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    // Enum to define scene indices for clarity and maintainability
    public enum SceneIndex
    {
        MainMenuSceneIndex = 0,
        GameSceneSceneIndex = 1
    }

    /// <summary>
    /// Start a new round.
    /// </summary>
    public void StartGame()
    {
        StartCoroutine(LoadSceneAdditively((int)SceneIndex.GameSceneSceneIndex));
    }


    /// <summary>
    /// Load the main menu.
    /// </summary>
    public void LoadMainMenu()
    {
        StartCoroutine(LoadSceneAdditively((int)SceneIndex.MainMenuSceneIndex));
    }

    // Coroutine to load a new scene additively and unload the current active scene
    private IEnumerator LoadSceneAdditively(int sceneBuildIndex)
    {
        // Get the currently active scene
        Scene activeScene = SceneManager.GetActiveScene();

        // Load the new scene additively
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneBuildIndex, LoadSceneMode.Additive);

        // Wait until the new scene has loaded
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // Unload the previously active scene
        SceneManager.UnloadSceneAsync(activeScene);
    }
}