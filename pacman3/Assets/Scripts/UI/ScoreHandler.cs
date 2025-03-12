using TMPro;
using UnityEngine;

public class ScoreHandler : MonoBehaviour {
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
        UpdateScore(0);
    }

    private void UpdateScore(float value) {
        score.text = value.ToString("00");
    }
}
