using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreCounter : MonoBehaviour
{
    [SerializeField]
    float scoreRate;

    [SerializeField]
    TMP_Text highscore;

    float score;

    TMP_Text scoreText;

    void Start()
    {
        scoreText = GetComponent<TMP_Text>();    
    }

    void Update()
    {
        score += Time.deltaTime * scoreRate;

        scoreText.text = (int)score + "";

        if ((int)score > PlayerPrefs.GetInt("Highscore", 0))
        {
            PlayerPrefs.SetInt("Highscore", (int)score);
        }

        highscore.text = PlayerPrefs.GetInt("Highscore", 0) + "";
    }
}
