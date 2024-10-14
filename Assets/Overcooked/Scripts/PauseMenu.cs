using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public bool isPaused;
    public GameObject firstButton;

    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
        isPaused = false;
    }

    private void OnEnable()
    {
        SetFirstSelectedButton();
    }

    private void SetFirstSelectedButton()
    {
        EventSystem.current.SetSelectedGameObject(null); // Deselect anything currently selected
        EventSystem.current.SetSelectedGameObject(firstButton); // Select the first button
    }

    public void GoMenu()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("Main Menu");

    }

    public void Quit()
    {
        Application.Quit();
    }

    public void PauseGame()
    {
        if(!isPaused)
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0f;
            isPaused = true;
            SetFirstSelectedButton();
            MultiplayerInputManager.instance.DisableInputs();
            //This just stops people from doing things when the game is paused
        }
        else if(isPaused)
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 1f;
            isPaused = false;
            EventSystem.current.SetSelectedGameObject(null);
            MultiplayerInputManager.instance.EnableInputs();
            //Now they can move again :)
        }


    }

}
