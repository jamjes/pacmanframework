using System.Collections;
using UnityEngine;
using static UnityEngine.CullingGroup;

public class Blinky : MonoBehaviour
{
    private Pathfinding pathfinding;
    public LayerMask wallLayer;
    private Vector2 currentDirection;
    private GameObject pacman;
    private bool? isMoving = false;
    private GridMovement movement;
    public float duration;
    private Vector2 startPosition;
    public Vector2 endPosition;
    private bool run;
    private Vector2 spawnPosition;
    public Vector2 homePosition;

    public enum State {
        Chase,
        Scatter
    };

    public State currentState = State.Chase;

    private void Awake() {
        spawnPosition = transform.position;
        pathfinding = new Pathfinding(transform, wallLayer);
        movement = new GridMovement(duration);
        pacman = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(DelayStart());
    }

    private void OnEnable() {
        Pacman.OnPlayerDeath += Respawn;
        GhostStateManager.OnScatterEnter += StateChange;
        GhostStateManager.OnChaseEnter += StateChange;
    }

    private void OnDisable() {
        Pacman.OnPlayerDeath -= Respawn;
        GhostStateManager.OnScatterEnter -= StateChange;
        GhostStateManager.OnChaseEnter -= StateChange;
    }

    private void Update() {
        if (run == false) {
            return;
        }

        if (currentState == State.Chase) {
            ChaseUpdate();
        } else if (currentState == State.Scatter) {
            ScatterUpdate();
        }
        
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, currentDirection, .51f);
        foreach(RaycastHit2D hit in hits) {
            if (hit.collider.tag == "Teleport") {
                transform.position = hit.collider.GetComponent<Teleport>().TeleportPoint;
                isMoving = false;
            }
        }

        if (isMoving == true) {
            isMoving = !movement.MoveTo(transform, startPosition, endPosition);
        }
    }

    private void ChaseUpdate() {
        if (isMoving == false) {
            currentDirection = pathfinding.GetShortestDirection(pacman.transform.position, currentDirection);
            startPosition = transform.position;
            endPosition = startPosition + currentDirection;
            isMoving = true;
        }
    }

    private void ScatterUpdate() {
        if (isMoving == false) {
            currentDirection = pathfinding.GetShortestDirection(homePosition, currentDirection);
            startPosition = transform.position;
            endPosition = startPosition + currentDirection;
            isMoving = true;
        }
    }

    private IEnumerator DelayStart() {
        yield return new WaitForSeconds(3);
        run = true;
    }

    private void Respawn() {
        transform.position = spawnPosition;
        isMoving = false;
        run = false;
        StartCoroutine(DelayStart());
    }

    private void StateChange(GhostStateManager.GhostState newState) {
        switch (newState) {
            case GhostStateManager.GhostState.Scatter:
                currentState = State.Scatter; break;
            case GhostStateManager.GhostState.Chase:
                currentState = State.Chase; break;
        }
    }
}
