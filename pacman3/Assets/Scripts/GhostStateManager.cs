using System.Collections.Generic;
using UnityEngine;

public class GhostStateManager : MonoBehaviour
{
    public float[] stateflow;
    private int index = 0;
    private bool run = true;
    public enum GhostState {
        Chase,
        Scatter,
        Frightened,
        Eaten
    };

    public GhostState CurrentState;

    float elapsedTime;

    public delegate void GhostStateEvent(GhostState targetState);
    public static event GhostStateEvent OnScatterEnter;
    public static event GhostStateEvent OnChaseEnter;
    private void Start() {
        CurrentState = GhostState.Scatter;
        if (OnScatterEnter != null) {
            OnScatterEnter(CurrentState);
        }
    }

    void Update()
    {
        if (run == false) {
            return;
        }

        elapsedTime += Time.deltaTime;
        
        
    }

    void Pause() {
        if (run != false)
        run = false;
    }

    void Play() {
        if (run != true)
        run = true;
    }
}
