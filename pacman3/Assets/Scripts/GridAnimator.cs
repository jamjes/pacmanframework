using System.Collections;
using UnityEngine;

public class GridAnimator : MonoBehaviour
{
    public GameObject overlayGrid;

    private void OnEnable() {
        Pacman.OnPlayerWin += BeginAnimation;
    }

    private void OnDisable() {
        Pacman.OnPlayerWin -= BeginAnimation;
    }

    private void Awake() {
        overlayGrid.SetActive(false);
    }

    private void BeginAnimation() {
        StartCoroutine(Animate());
    }

    private IEnumerator Animate() {
        yield return new WaitForSeconds(.2f);
        overlayGrid.SetActive(true);
        yield return new WaitForSeconds(.2f);
        overlayGrid.SetActive(false);
        yield return new WaitForSeconds(.2f);
        overlayGrid.SetActive(true);
        yield return new WaitForSeconds(.2f);
        overlayGrid.SetActive(false);
    }
}
