using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEditor.Experimental.GraphView.GraphView;

public class GameManager : MonoBehaviour
{
    public MultiplayerInputManager inputManager;
    public string currentScene;


    private void Awake()
    {
        inputManager = FindObjectOfType<MultiplayerInputManager>();
        currentScene = SceneManager.GetActiveScene().name;    }
    public void GameEnded()
    {
        inputManager.DisableInputs();
        
        StartCoroutine(ResetGame());
    }
    IEnumerator ResetGame()
    {
        yield return new WaitForSecondsRealtime(3f);
        SceneManager.LoadScene(currentScene);
    }
}
