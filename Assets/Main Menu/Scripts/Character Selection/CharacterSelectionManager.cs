using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;


public class CharacterSelectionManager : MonoBehaviour
{
    public Animator animator;
    private string sceneName;

    public bool skipTutorial = false;

    private void Awake()
    {
        Scene scene = SceneManager.GetActiveScene();


        CharacterDataManager.instance.ResetActivePlayers();

        PlayerPrefs.SetInt("tutorial", 1);
        PlayerPrefs.Save();

        if (scene.name == "Character Selection")
        {
            MultiplayerInputManager.instance.ClearPlayers();
            MultiplayerInputManager.instance.InitializeInputs();
        }
    }
    private void Start()
    {
        PlayerPrefs.SetInt("tutorial", 0);
        PlayerPrefs.Save();
    }


    public void LevelFader()
    {
        animator.SetTrigger("FadeOut");
    }


    public void LoadScene()
    {

        SceneManager.LoadSceneAsync(sceneName);

    }

    public void SetSceneToMenu()
    {
        AudioManager.instance.sfxSourceNonPlayer.Stop();
        AudioManager.instance.PlaySFX("Click Button", 4);
        sceneName = "Main Menu";
    }

    public void SetSceneToMainGame()
    {
        AudioManager.instance.PlaySFX("Character Select Start Game", 4, 1.2f);
        sceneName = "Don't Stop Smithing";
    }

    public void SetSceneToCharactSelct()
    {
        AudioManager.instance.sfxSourceNonPlayer.Stop();
        AudioManager.instance.PlaySFX("Click Button", 4);
        sceneName = "Character Selection";
    }


    public void Quit()
    {
        Application.Quit();
    }

    public void IsTutorialOn(bool tutorialbool)
    {
        PlayerPrefs.SetInt("tutorial", tutorialbool ? 1 : 0);
        PlayerPrefs.Save();
    }


    public void PlayButtonSound()
    {
        AudioManager.instance.PlaySFX("Select Button", 4);
    }
}
