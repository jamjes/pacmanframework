using System.Collections;
using UnityEngine;

public class LevelDialogue : MonoBehaviour
{
    public GameObject startText;
    public GameObject deathText;
    
    private void Start() {
        StartCoroutine(DisplayStartText());
    }

    private void OnEnable() {
        Pacman.OnPlayerDamage += Trigger;
        Pacman.OnPlayerDeath += DisplayEndText;
    }

    private void OnDisable() {
        Pacman.OnPlayerDamage -= Trigger;
        Pacman.OnPlayerDeath -= DisplayEndText;
    }

    private IEnumerator DisplayStartText() {
        startText.SetActive(true);
        yield return new WaitForSeconds(3f);
        startText.SetActive(false);
    }

    private void Trigger() {
        StartCoroutine(DisplayStartText());
    }

    private void DisplayEndText() {
        deathText.SetActive(true);
    }
}
