using System.Collections;
using UnityEngine;

public class Pacman : MonoBehaviour
{
    public int Score { get; private set; }
    public int Lives { get; private set; }
    [SerializeField] private float moveDuration;
    public Vector2 TargetDirection { get; private set; }
    private Vector2 currentDirection;
    private GridMovement movement;
    public LayerMask wallLayer;
    private Vector2 startPosition, targetPosition;
    private bool? isMoving = false;
    public delegate void UIEvent(Pacman self);
    public static event UIEvent OnScoreUpdate;
    public delegate void PlayerEvent();
    public static event PlayerEvent OnPowerPelletEat;
    public static event PlayerEvent OnPlayerDeath;
    public static event PlayerEvent OnPlayerWin;
    private bool run;
    private int totalPellets;

    public enum State {
        Chase,
        Frightened
    };

    public State gameState;

    private void Awake() {
        movement = new GridMovement(moveDuration);
    }

    private void Start() {
        int pellets = GameObject.FindGameObjectsWithTag("Pellet").Length;
        int powerPellets = GameObject.FindGameObjectsWithTag("Power Pellet").Length;
        totalPellets = pellets + powerPellets;
        TargetDirection = Vector2.right;
        Score = 0;
        Lives = 3;
        StartCoroutine(DelayStart());
    }

    private void Update() {
        ListenForInputs();

        if (run == false) {
            return;
        }
        
        CollisionChecks();
        
        if (isMoving == false) {
            Vector2 direction = CalculateDirection();
            if (direction != Vector2.zero) {
                startPosition = transform.position;
                targetPosition = startPosition + direction;
                currentDirection = direction;
                isMoving = true;
            }
        } else if (isMoving == true){
            isMoving = !movement.MoveTo(transform, startPosition, targetPosition);
        }
    }

    private void ListenForInputs() {
        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            TargetDirection = Vector2.up;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            TargetDirection = Vector2.left;
        }

        if (Input.GetKeyDown(KeyCode.DownArrow)) {
            TargetDirection = Vector2.down;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            TargetDirection = Vector2.right;
        }
    }

    private Vector2 CalculateDirection() {
        Vector2 direction = Vector2.zero;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, TargetDirection, .55f, wallLayer);
        if (hit.collider == null) { //Intended direction is not a wall
            direction = TargetDirection;
        }
        else if (currentDirection != Vector2.zero) {
            hit = Physics2D.Raycast(transform.position, currentDirection, .55f, wallLayer);
            if (hit.collider == null) { //Continuing in previous direction is not a wall
                direction = currentDirection;
            }
        }

        return direction;
    }

    private void CollisionChecks() {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, currentDirection, .5f);
        if (hit.collider == null) {
            return;
        }
        
        if ((hit.collider.tag == "Pellet" || hit.collider.tag == "Power Pellet")) {
            totalPellets--;
            UpdateScore(hit.collider.tag);
            Destroy(hit.collider.gameObject);
            if (totalPellets == 0) {
                run = false;
                if (OnPlayerWin != null) { 
                    OnPlayerWin();
                }
            }
        } else if (hit.collider.tag == "Ghost") {
            Death();
        } else if (hit.collider.tag == "Teleport") {
            transform.position = hit.collider.GetComponent<Teleport>().TeleportPoint;
            isMoving = false;
        }
    }

    private void UpdateScore(string tag) {
        if (tag == "Pellet") {
            Score += 10;
        } else if (tag == "Power Pellet") {
            Score += 50;
            gameState = State.Frightened;
            if (OnPowerPelletEat != null) {
                OnPowerPelletEat();
            }
        }

        if (OnScoreUpdate != null) {
            OnScoreUpdate(this);
        }
    }

    private void CheckGhostCondition(Collider2D ghost) {
        if (gameState == State.Frightened) {
            Score += 200;
            if (OnScoreUpdate != null) {
                OnScoreUpdate(this);
            }
        }
    }

    private void Death() {
        run = false;
        if (OnPlayerDeath != null) OnPlayerDeath();
        Lives--;
        if (Lives > 0) {
            transform.position = startPosition = targetPosition = Vector2.zero;
            currentDirection = TargetDirection = Vector2.zero;
            StartCoroutine(DelayStart());
        }
    }

    private IEnumerator DelayStart() {
        yield return new WaitForSeconds(3);
        if (TargetDirection == Vector2.zero) TargetDirection = Vector2.right;
        run = true;
    }
}
