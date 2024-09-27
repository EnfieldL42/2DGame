using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class CharacterSelectionManager : MonoBehaviour
{
    public Animator animator;
    private string sceneName;

    public bool tutorial = true;

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

    public void SetDifficulty(bool tutorialbool)
    {
        tutorial = tutorialbool;
        PlayerPrefs.SetInt("tutorial", tutorial ? 1 : 0);
        PlayerPrefs.Save();
    }

}
