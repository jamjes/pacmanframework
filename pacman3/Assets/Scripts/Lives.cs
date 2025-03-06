using UnityEngine;
using UnityEngine.UI;

public class Lives : MonoBehaviour
{
    public Image[] lives;
    int pointer;
    public GameObject gameOverText;

    private void Start() {
        pointer = lives.Length;
        gameOverText.SetActive(false);
    }

    private void OnEnable() {
        Pacman.OnPlayerDeath += Decrement;
    }

    private void OnDisable() {
        Pacman.OnPlayerDeath -= Decrement;
    }

    private void Decrement() {
        if (pointer > 0) {
            pointer--;
            lives[pointer].enabled = false;
        } else {
            gameOverText.SetActive(true);
        }
    }
}
