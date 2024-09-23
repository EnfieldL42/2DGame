using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OCGameManager : MonoBehaviour
{
    public float gameDuration;
    public float timer;
    public List<int> playerScores = new List<int>();
    public List<GameObject> playerGameObjects = new List<GameObject>();

    [SerializeField] private Dictionary<int, int> uniqueItemScores = new Dictionary<int, int>();
    public List<int> uniqueScores = new List<int>();
    public List<int> startingScores = new List<int>();
    public List<int> scores = new List<int>();
    public int whichRound = 0; //hh


    public Animator animator;

    private void Awake()
    {
        timer = gameDuration;
    }

    void Start()
    {
        MultiplayerInputManager.instance.onPlayerJoined += OnPlayerJoined;

        EnablePlayers();
        InitializePlayerScores();
        DisableLosers(); 

        ChooseScores();
    }

    void ChooseScores()
    {
        //goes through all your uniqueScores to add random Score
        for (int i = 0; i < 4; i++)
        {
            int randomScore = Random.Range(0, startingScores.Count);
            uniqueItemScores[i] = startingScores[randomScore];
            uniqueScores[i] = startingScores[randomScore];
        }
    }

    void SetSingleItem(int id)
    {
        int randomScore = Random.Range(0, scores.Count);
        uniqueItemScores[id] = scores[randomScore];
        uniqueScores[id] = scores[randomScore];
    }

    private void OnDestroy()
    {
        MultiplayerInputManager.instance.onPlayerJoined -= OnPlayerJoined;
    }

    private void OnPlayerJoined(int playerID)
    {
        if (playerID >= playerScores.Count)
        {
            playerScores.Add(0);
        }

        if (playerID < playerGameObjects.Count)
        {
            playerGameObjects[playerID].SetActive(true);
        }
    }

    private void EnablePlayers()
    {
        for (int i = 0; i < MultiplayerInputManager.instance.players.Count; i++)
        {
            if (i < playerGameObjects.Count)
            {
                playerGameObjects[i].SetActive(true);
            }
        }
    }

    void Update()
    {
        StartTimer();
    }

    public void StartTimer()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                timer = 0;
            }
        }
        else
        {
            StartNextRound();
        }
    }

    public void StartNextRound()
    {
        SetBallPlayers();
    }

    void SetBallPlayers()
    {
        List<(int ID, int Score)> playerScoresList = new List<(int ID, int Score)>();

        for (int i = 0; i < playerScores.Count; i++)
        {
            if (playerGameObjects[i].activeSelf) // Only consider active players
            {
                playerScoresList.Add((i, playerScores[i]));
            }
        }

        playerScoresList.Sort((x, y) => x.Score.CompareTo(y.Score));

        if (playerScoresList.Count >= 2)
        {
            int lowestScoreID = playerScoresList[0].ID;
            int secondLowestScoreID = playerScoresList[1].ID;

            GameManager.SetPlayerOne(lowestScoreID);
            GameManager.SetPlayerTwo(secondLowestScoreID);

            SceneFade();

        }
        else
        {
            Debug.LogWarning("Not enough active players for deathmatch");
        }
    }

    public void SceneFade()
    {
        animator.SetTrigger("FadeOut");
    }

    public void LoadScene()
    {
        SceneManager.LoadScene("Ball Game Test");

    }


    private void DisableLosers()
    {
        foreach (int loserID in GameManager.losers)
        {
            if (loserID < playerGameObjects.Count)
            {
                playerGameObjects[loserID].SetActive(false);
                playerScores[loserID] = int.MaxValue; // Set the loser's score to a high value to exclude them
            }
        }
    }

    public void AddScore(int playerID, int uniqueItemID)
    {
        if (uniqueItemScores.TryGetValue(uniqueItemID, out int score))
        {
            playerScores[playerID] += score;
            SetSingleItem(uniqueItemID);
            Debug.Log($"Added {score} points to player {playerID} for using unique item {uniqueItemID}");
        }
        else
        {
            Debug.LogWarning($"No score found for unique item ID {uniqueItemID}");
        }
    }

    private void InitializePlayerScores()
    {
        playerScores.Clear();
        for (int i = 0; i < MultiplayerInputManager.instance.players.Count; i++)
        {
            playerScores.Add(0);
        }
    }
}
