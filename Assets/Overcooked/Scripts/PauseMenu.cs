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
    public OCGameManager gm;
    public GameObject winScreen;


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
        GameManager.losers.Clear();
        Time.timeScale = 1.0f;
        pauseMenu.SetActive(false);
        gm.SetSceneMainMenu();
        gm.SceneFade();

        AudioManager.instance.UnpauseAllAudio();
        AudioManager.instance.StopAllAudio();

    }

    public void Quit()
    {
        Application.Quit();
    }

    public void PauseGame()
    {
        if(!winScreen.gameObject.activeSelf)
        {
            if (!isPaused)
            {
                pauseMenu.SetActive(true);
                Time.timeScale = 0f;
                isPaused = true;
                SetFirstSelectedButton();
                AudioManager.instance.PauseAllAudio();
                gm.UnlockMouse();

                MultiplayerInputManager.instance.DisableInputs();
                //This just stops people from doing things when the game is paused
            }
            else if (isPaused)
            {
                if (pauseMenu != null)
                {
                    pauseMenu.SetActive(false);
                    Time.timeScale = 1f;
                    isPaused = false;
                    EventSystem.current.SetSelectedGameObject(null);
                    AudioManager.instance.UnpauseAllAudio();
                    gm.LockMouse();

                    if (gm.canPauseWalk)
                    {
                        MultiplayerInputManager.instance.EnableInputs();
                    }
                }
            }

        }


    }

    public void PlayButtonClickSound()
    {
        AudioManager.instance.PlaySFX("Click Button", 4);
    }
}
