using System.Collections;
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
        StartCoroutine(DecrementAfterDelay());
    }

    private IEnumerator DecrementAfterDelay() {
        yield return new WaitForSeconds(3);
        lives[pointer].enabled = false;
        pointer--;
    }
}
