using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    TextMeshProUGUI scoreLabel;

    private void Awake() {
        scoreLabel = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable() {
        Pacman.OnScoreUpdate += UpdateScore;
    }

    private void OnDisable() {
        Pacman.OnScoreUpdate += UpdateScore;
    }

    private void UpdateScore(Pacman pacman) {
        scoreLabel.text = pacman.Score.ToString("0000");
    }
}
