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



    /*
    // Replace these constants with your actual scene indices from the Build Settings
    private const int MainMenuSceneIndex = 0;
    private const int GameSceneIndex = 1;

    // Function to start the game
    public void StartGame()
    {
        StartCoroutine(LoadGameScene());
    }

    // Coroutine to load the game scene additively and unload the main menu
    private IEnumerator LoadGameScene()
    {
        // Load the GameScene additively
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(GameSceneIndex, LoadSceneMode.Additive);

        // Wait until the game scene has loaded
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // Optionally, you can also activate objects or set up the game scene here

        // Unload the MainMenu scene
        SceneManager.UnloadSceneAsync("MainMenu");
    }

    public void LoadMainMenu()
    {
        StartCoroutine(LoadMenuScene());
    }

    private IEnumerator LoadMenuScene()
    {
        // Load the MainMenu scene additively
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(MainMenuSceneIndex, LoadSceneMode.Additive);

        // Wait until the MainMenu scene has loaded
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // Optionally, you can also activate objects or set up the menu scene here

        // Unload the GameScene
        SceneManager.UnloadSceneAsync("GameScene");
    }*/
}