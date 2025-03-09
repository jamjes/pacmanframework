using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour
{
    private bool run;
    public float elapsedTime;
    public float[] modeDuration;
    int pointer;

    public delegate void GhostEvent(string state);
    public static event GhostEvent OnStateChange;

    private void Start() {
        StartCoroutine(DelayStart());
    }

    private void Update() {
        if (run == false) {
            return;
        }

        if (OnStateChange == null) {
            return;
        }

        elapsedTime += Time.deltaTime;

        if (elapsedTime >= modeDuration[pointer]) {
            pointer++;
            if (pointer == modeDuration.Length) {
                run = false;
                OnStateChange("chase");
                Debug.Log("End");
            } else {
                if (pointer % 2 == 0) {
                    OnStateChange("scatter");
                } else {
                    OnStateChange("chase");
                }
                elapsedTime = 0;
            }
        }
    }

    private IEnumerator DelayStart() {
        yield return new WaitForSeconds(3);
        run = true;
    }

    private void PauseTime() {
        if (run == true) {
            run = false;
            StartCoroutine(DelayStart());
        }
    }
}
