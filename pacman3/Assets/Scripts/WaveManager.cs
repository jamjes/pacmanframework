using UnityEngine;
using CustomVariables;
using System.Collections;
using Unity.VisualScripting;

public class WaveManager : MonoBehaviour
{
    public Ghost[] ghosts;
    private bool run = false;
    
    private float[] spawnTimes = {8, 16, 24 };
    private int ghostIndex;
    public float elapsedTime;

    private float[] stateTimes = { 7, 27, 34, 54, 59, 79, 84 };
    private int stateIndex;


    private void OnEnable() {
        Pacman.OnPlayerDamage += ResetObject;
        Pacman.OnPlayerDeath += Stop;
        Pacman.OnPlayerDeath += Stop;
        Pacman.OnPlayerWin += Stop;
    }

    private void OnDisable() {
        Pacman.OnPlayerDamage -= ResetObject;
        Pacman.OnPlayerDeath -= Stop;
        Pacman.OnPlayerDeath -= Stop;
        Pacman.OnPlayerWin -= Stop;
    }

    private void Start() {
        StartCoroutine(DelayStart());
    }

    private void Update() {
        if (run == false) {
            return;
        }

        if (ghostIndex != spawnTimes.Length) {
            if (Mathf.Floor(elapsedTime) == spawnTimes[ghostIndex]) {
                ghosts[ghostIndex + 1].Enable();
                ghostIndex++;
            }
        }

        if (stateIndex != stateTimes.Length) {
            if (Mathf.Floor(elapsedTime) == stateTimes[stateIndex]) {
                foreach (Ghost ghost in ghosts) {
                    if (ghost.CurrentState == GhostState.Frightened) {
                        break;
                    }

                    if (stateIndex % 2 == 0) {
                        ghost.SetState(GhostState.Chase);
                    }
                    else {
                        ghost.SetState(GhostState.Scatter);
                    }
                }
                stateIndex++;
            }
        }

        elapsedTime += Time.deltaTime;
    }

    private void ResetObject() {
        ghostIndex = 0;
        stateIndex = 0;
        elapsedTime = 0;
        run = false;
        StartCoroutine(DelayStart());
    }

    private IEnumerator DelayStart() {
        yield return new WaitForSeconds(3);
        run = true;
    }

    private void Stop() {
        run = false;
    }
}
