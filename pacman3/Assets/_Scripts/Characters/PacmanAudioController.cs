using System.Collections;
using UnityEngine;

public class PacmanAudioController : MonoBehaviour
{
    public Pacman self;
    public AudioSource src;
    public AudioClip eat, death;

    private void OnEnable() {
        Pacman.OnPlayerDamage += OnDamage;
        Pacman.OnPlayerDeath += OnDamage;
    }

    private void OnDisable() {
        Pacman.OnPlayerDamage -= OnDamage;
        Pacman.OnPlayerDeath -= OnDamage;
    }

    private void Update() {
        if (self.run && self.Direction != Vector2.zero) {
            if (src.clip != eat) {
                src.clip = eat;
            }
            
            if (!src.isPlaying) {
                src.Play();
            }
        }
    }

    private void OnDamage() {
        src.Stop();
        StartCoroutine(DeathSound());
    }

    private IEnumerator DeathSound() {
        yield return new WaitForSeconds(.8f);
        src.clip = death;
        src.Play();
    }
}
