using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class Lives : MonoBehaviour
{
    TextMeshProUGUI livesLabel;
    int lives = 3;

    private void Awake() {
        livesLabel = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable() {
        Player.OnPlayerDeath += Decrement;
    }

    private void OnDisable() {
        Player.OnPlayerDeath -= Decrement;
    }

    private void Decrement() {
        lives--;
        livesLabel.text = lives.ToString();
    }
}
