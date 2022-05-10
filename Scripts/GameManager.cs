using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [NonSerialized]
    public GenerateLevel levelGenerator;
    public Text scoreText, handHeldScore;

    public int score = 0;
    public int scoreAddition = 1000;
    public bool doorClosed = false;
    public List<GameObject> respawnableItems;

    private void Awake()
    {
        if (instance)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        levelGenerator = GetComponent<GenerateLevel>();
    }
    private void Start()
    {
        levelGenerator.Generate();
    }
    public void ResetLevel()
    {
        GameObject[] items = GameObject.FindGameObjectsWithTag("Item");
        foreach(GameObject item in items)
        {
            Destroy(item);
        }
        respawnableItems.Clear();
        levelGenerator.Generate();
        score = 0;
        scoreText.text = "Score: 0";
        handHeldScore.text = "Score: 0";
        scoreAddition = 1000;
    }

    public void GetPoints()
    {
        score += scoreAddition;
        scoreText.text = "Score: " + score;
        handHeldScore.text = "Score: " + score;
    }
    public void TryAgain()
    {
        scoreAddition /= 2;
        levelGenerator.RespawnItems(respawnableItems);
    }
}