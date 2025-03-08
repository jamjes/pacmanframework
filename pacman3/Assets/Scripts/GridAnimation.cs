using System.Collections;
using UnityEngine;

public class GridAnimation : MonoBehaviour
{
    public GameObject OverlayGrid;

    private void OnEnable() {
        Pacman.OnPlayerWin += WinAnimate;
    }

    private void OnDisable() {
        Pacman.OnPlayerWin -= WinAnimate;
    }

    private void Awake() {
        OverlayGrid.SetActive(false);
    }

    private void WinAnimate() {
        StartCoroutine(Animate());
    }

    private IEnumerator Animate() {
        yield return new WaitForSeconds(.3f);
        OverlayGrid.SetActive(true);
        yield return new WaitForSeconds(.3f);
        OverlayGrid.SetActive(false);
        yield return new WaitForSeconds(.3f);
        OverlayGrid.SetActive(true);
        yield return new WaitForSeconds(.3f);
        OverlayGrid.SetActive(false);
    }
}
