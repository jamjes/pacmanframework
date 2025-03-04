using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    TextMeshProUGUI scoreLabel;
    int score;

    private void Awake() {
        scoreLabel = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable() {
        Player.OnScoreUpdate += UpdateScore;
    }

    private void OnDisable() {
        Player.OnScoreUpdate += UpdateScore;
    }

    private void UpdateScore(int score) {
        this.score = score;
        scoreLabel.text = score.ToString("0000");
    }
}
