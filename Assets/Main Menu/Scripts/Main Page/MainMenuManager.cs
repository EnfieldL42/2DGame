using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public Animator animator;
    public string sceneName;

    public void LevelFader()
    {
        animator.SetTrigger("FadeOut");
    }

    public void LoadScene()
    {
        SceneManager.LoadSceneAsync(sceneName);
    }


    public void SetSceneToCharactSelct()
    {
        sceneName = "Character Selection";
    }


    public void Quit()
    {
        Application.Quit();
    }


}
