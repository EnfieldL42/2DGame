using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDataManager : MonoBehaviour
{
    //static = there can be one of these across all of the same script
    public static CharacterDataManager instance;
    public int[] characterSelection;

    public bool[] activePlayers;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetPlayer(int playerIndex, int spriteIndex)
    {
        characterSelection[playerIndex] = spriteIndex;
    }

    public int GetCharacterSprite(int playerIndex)
    {
        return characterSelection[playerIndex]; 
    }

    public void EliminatePlayer(int playerIndex)
    {
        if (playerIndex >= 0 && playerIndex < activePlayers.Length)
        {
            activePlayers[playerIndex] = false;
        }
    }

    public void TurnOnPlayer(int playerIndex)
    {
        if (playerIndex >= 0 && playerIndex < activePlayers.Length)
        {
            activePlayers[playerIndex] = true;
        }
    }

    public void ResetActivePlayers()
    {
        for (int i = 0; i < activePlayers.Length; i++)
        {
            activePlayers[i] = false; 
        }
    }

    public bool IsPlayerActive(int playerIndex)
    {
        return activePlayers[playerIndex];
    }
}
