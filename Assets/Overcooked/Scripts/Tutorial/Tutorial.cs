using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Tilemaps;

public class Tutorial : MonoBehaviour
{
    public TextMeshProUGUI dialogue;
    public Animator anim;
    public Animator ingredients;
    public OCGameManager gm;

    public string[] lines;
    public float textSpeed;
    public float waitTime;

    private int index;
    private bool isTyping = false;

    public GameObject tileClosed;
    public GameObject tileAnimation;
    //public GameObject tileOpen;


    void Start()
    {
        dialogue.text = string.Empty;
        //StartDialogue();
    }

    void Update()
    {
        // Wait until typing is done and we're not already waiting for the next line
        if (dialogue.text == lines[index] && !isTyping)
        {
            StartCoroutine(InbetweenLinesWait());
        }
    }

    void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator InbetweenLinesWait()
    {
        isTyping = true;
        yield return new WaitForSeconds(waitTime);
        NextLine();
    }

    IEnumerator TypeLine()
    {
        isTyping = true; // Ensure we don’t start the next line until this one is fully typed
        foreach (char c in lines[index].ToCharArray())
        {
            dialogue.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
        isTyping = false; 
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            dialogue.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            //gameObject.SetActive(false);
            anim.SetTrigger("TutorialDone");
        }
    }

    public void StartTimer()
    {
        gm.StartTimer();
    }

    /*public void TileOpenStartingArea()
    {
        tileClosed.SetActive(false);
        tileAnimation.SetActive(true);
        tileAnimation.GetComponent<Tilemap>().animationFrameRate = 8;
        //Had to this because the tile is still animated when inactive
    } */

    public void IngredientsAnimation()
    {
        ingredients.SetTrigger("animate");
    }

    /*public IEnumerator gateOpen()
    {
        yield return new WaitForSeconds(1f);
        //tileOpen.SetActive(true);
        tileAnimation.SetActive(false);

    }*/
}
