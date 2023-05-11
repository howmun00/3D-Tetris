using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    int score;
    int level;
    int layersCleared;

    bool gameIsOver;

    float fallSpeed;

    public GameObject gameOver;

    void Awake()
    {
        instance = this;
    } 

    void Start()
    {
        gameOver.SetActive(false);
        SetScore(score);
    }

    public void SetScore(int amount)
    {
        score += amount;
        CalculateLevel();
        UIHandler.instance.UpdateUI(score, level, layersCleared);
        //update ui
    }

    public float ReadFallSpeed()
    {
        return fallSpeed;
    }

    public void LayersCleared(int amount)
    {
        if(amount == 1)
        {
            SetScore(400);
        }
        if(amount == 2)
        {
            SetScore(900);
        }
        if(amount == 3)
        {
            SetScore(1400);
        }
        if(amount == 4)
        {
            SetScore(2000);
        }
        if(amount == 5)
        {
            SetScore(2500);
        }

        layersCleared += amount;
        //update ui
        UIHandler.instance.UpdateUI(score, level, layersCleared);
    }

    void CalculateLevel()
    {
        if(score <= 1000)
        {
            level = 1;
            fallSpeed = 3f;
        }
        else if(score >= 1000 && score <= 2000)
        {
            level = 2;
            fallSpeed = 2.5f;
        }
        else if(score >= 2000 && score <= 3000)
        {
            level = 3;
            fallSpeed = 2f;
        }
        else if(score >= 3000 && score <= 4000)
        {
            level = 4;
            fallSpeed = 1.5f;
        }
        else if(score >= 4000 && score <= 5000)
        {
            level = 5;
            fallSpeed = 1f;
        }
    }

    public bool ReadGameIsOver()
    {
        return gameIsOver;
    }

    public void SetGameIsOver()
    {
        gameIsOver = true;
        gameOver.SetActive(true);
    }
}
