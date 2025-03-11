using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    private bool run;
    public float elapsedTime;
    public float[] modeDuration;
    int pointer;
    public GameObject gate;

    public delegate void GhostEvent(string state);
    public static event GhostEvent OnStateChange;

    public Ghost[] ghosts;

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
        //if (Mathf.Floor(elapsedTime) == 8 && ghosts[1].CurrentState != Ghost.State.Init) {
        //    ghosts[1].CurrentState = Ghost.State.Init;
        //} else if (Mathf.Floor(elapsedTime) == 17 && ghosts[2].CurrentState != Ghost.State.Init) {
        //    ghosts[2].CurrentState = Ghost.State.Init;
        //} else if (Mathf.Floor(elapsedTime) == 26 && ghosts[3].CurrentState != Ghost.State.Init) {
        //    ghosts[3].CurrentState = Ghost.State.Init;
        //}

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
