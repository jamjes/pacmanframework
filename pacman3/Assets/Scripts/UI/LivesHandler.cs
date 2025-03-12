using UnityEngine;
using UnityEngine.UI;

public class LivesHandler : MonoBehaviour
{
    public Image[] lives;
    private int pointer;

    private void OnEnable() {
        Pacman.OnPlayerDamage += DecrementLives;
    }

    private void OnDisable() {
        Pacman.OnPlayerDamage -= DecrementLives;
    }

    private void Start() {
        pointer = lives.Length - 1;
    }

    private void DecrementLives() {
        lives[pointer].enabled = false;
        pointer--;
    }
}
