using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public Animator animator;


    public void LevelFader()
    {
        animator.SetTrigger("FadeOut");
    }

        public void startGame()
    {
        SceneManager.LoadSceneAsync("Character Selection");

    }
}
