using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIHandler : MonoBehaviour
{
    public static UIHandler instance;

    public TMP_Text scoreText;
    public TMP_Text levelText;
    public TMP_Text layersText;


    void Awake()
    {
        instance = this;
    }

    public void UpdateUI (int score, int level, int layers)
    {
        scoreText.text = "Score: " + score.ToString("D5");
        levelText.text = "Level: " + level.ToString("D2");
        layersText.text = "Layers: " + layers.ToString("D2");
    }
}
