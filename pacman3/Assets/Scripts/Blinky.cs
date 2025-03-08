using System.Collections;
using UnityEngine;

public class Blinky : MonoBehaviour
{
    private Pathfinding pathfinding;
    public LayerMask wallLayer;
    public Vector2 currentDirection;
    private GameObject pacman;
    private bool? isMoving = false;
    private GridMovement movement;
    public float duration;
    private Vector2 startPosition, endPosition;
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
        Pacman.OnPowerPelletEat += EnterScatter;
    }

    private void OnDisable() {
        Pacman.OnPlayerDeath -= Respawn;
        Pacman.OnPowerPelletEat -= EnterScatter;
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

    private void EnterScatter() {
        currentState = State.Scatter;
    }
}
