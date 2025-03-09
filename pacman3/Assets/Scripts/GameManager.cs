using System;
using UnityEngine;
using static GhostStateManager;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private bool run;
    private float elapsedTime;
    public float[] stateflow;
    private int index = 0;
    public delegate void GhostStateEvent(GhostState targetState);
    public static event GhostStateEvent OnScatterEnter;
    public static event GhostStateEvent OnChaseEnter;
    public enum GhostState {
        Chase,
        Scatter,
        Frightened,
        Eaten
    };

    public GhostState CurrentState = GhostState.Scatter;

    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(this.gameObject);
        }
    }

    private void Update() {
        if (run == false) {
            return;
        }
        
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= stateflow[index]) {
            if (CurrentState == GhostState.Scatter) {
                CurrentState = GhostState.Chase;
                if (OnChaseEnter != null) {
                    OnChaseEnter(CurrentState);
                }
            }
            else if (CurrentState == GhostState.Chase) {
                CurrentState = GhostState.Scatter;
                if (OnScatterEnter != null) {
                    OnScatterEnter(CurrentState);
                }
            }
            elapsedTime = 0;
            index++;
            if (index == stateflow.Length) {
                run = false;
            }
        }
    }

    public void Play() {
        if (run == false) run = true;
        Debug.Log("Resumed");
    }

    public void Pause() {
        if (run == true) run = false;
        Debug.Log("Paused");
    }
}
