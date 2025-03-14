using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEngine.Rendering.DebugUI;

public class HighScoreHandler : MonoBehaviour
{
    private TextMeshProUGUI score;

    private void Awake() {
        score = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable() {
        Pacman.OnScoreUpdate += UpdateScore;
    }

    private void OnDisable() {
        Pacman.OnScoreUpdate -= UpdateScore;
    }

    private void Start() {
        string text = $"HIGH SCORE \n{PlayerPrefs.GetInt("HighScore").ToString("0000")}";
        this.score.text = text;
    }

    private void UpdateScore(float currentScore) {
        if (currentScore > PlayerPrefs.GetInt("HighScore")) {
            SetHighScore((int)currentScore);
        }
    }

    private void SetHighScore(int value) {
        PlayerPrefs.SetInt("HighScore", value);
        string text = $"HIGH SCORE \n{PlayerPrefs.GetInt("HighScore").ToString("0000")}";
        this.score.text = text;
    }
}
