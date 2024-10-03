using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class CharacterSelectionManager : MonoBehaviour
{
    public Animator animator;
    private string sceneName;

    public bool skipTutorial = false;

    private void Awake()
    {
        PlayerPrefs.SetInt("tutorial", 1);
        PlayerPrefs.Save();
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
        sceneName = "Main Menu";
    }

    public void SetSceneToMainGame()
    {
        sceneName = "Don't Stop Smithing";
    }

    public void SetSceneToCharactSelct()
    {
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

}
