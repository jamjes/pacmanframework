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
        Pacman.OnPlayerDamage += TriggerAfterRespawn;
        Pacman.OnPlayerDeath += TriggerAfterDeath;
    }

    private void OnDisable() {
        Pacman.OnPlayerDamage -= TriggerAfterRespawn;
        Pacman.OnPlayerDeath -= TriggerAfterDeath;
    }

    private IEnumerator DisplayStartText() {
        startText.SetActive(true);
        yield return new WaitForSeconds(3f);
        startText.SetActive(false);
    }

    private IEnumerator DisplayStartTextAfterRespawn() {
        yield return new WaitForSeconds(3f);
        startText.SetActive(true);
        yield return new WaitForSeconds(3f);
        startText.SetActive(false);
    }

    private void TriggerAfterRespawn() {
        StartCoroutine(DisplayStartTextAfterRespawn());
    }

    private void TriggerAfterDeath() {
        StartCoroutine(DisplayEndText());
    }

    private IEnumerator DisplayEndText() {
        yield return new WaitForSeconds(3);
        deathText.SetActive(true);
    }
}
