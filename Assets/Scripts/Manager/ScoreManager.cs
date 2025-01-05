using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public Text scoreText;
    public TMPro.TextMeshProUGUI winText;
    public TMPro.TextMeshProUGUI loseText;
    public static int scoreCount;
    void Start()
    {
        scoreCount = 0;
    }

    void Update()
    {
        scoreText.text = "Score : " + scoreCount;
        loseText.text = "Votre score : " + scoreCount;
        winText.text = "Félicitation !\nVous avez gagné(e) avec un\nscore total de " + scoreCount;
    }
}
