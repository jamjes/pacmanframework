using System.Collections;
using System.Xml;
using Unity.VisualScripting;
using UnityEngine;

public class GhostAudioController : MonoBehaviour
{
    public WaveManager manager;
    public AudioSource src;
    public AudioSource chompSrc;
    public AudioClip chase;
    public AudioClip scatter;
    private bool isScatter;

    private void OnEnable() {
        Pacman.OnPowerPellet += Scatter;
        Ghost.OnGhostDeath += Chomp;
    }

    private void OnDisable() {
        Pacman.OnPowerPellet -= Scatter;
        Ghost.OnGhostDeath -= Chomp;
    }

    private void Update() {
        if (manager.run == false) {
            if (src.isPlaying && (src.clip == chase || src.clip == scatter)) {
                src.Stop();
            }

            return;
        }

        if (!src.isPlaying) {
            if (isScatter && src.clip != scatter) {
                src.clip = scatter;
            }
            else if (!isScatter && src.clip != chase) {
                src.clip = chase;
            }

            src.Play();
        }
    }

    private void Scatter() {
        isScatter = true;
        src.clip = scatter;
        StartCoroutine(ResetAfterScatter());
    }

    private IEnumerator ResetAfterScatter() {
        yield return new WaitForSeconds(7);
        isScatter = false;
        src.clip = chase;
    }

    private void Chomp() {
        StartCoroutine(PlayChomp());
    }

    private IEnumerator PlayChomp() {
        src.volume = 0;
        chompSrc.Play();
        yield return new WaitForSeconds(1);
        src.volume = 1;
    }
}
