using UnityEngine;
using System.Collections;

public class Ghost : MonoBehaviour, IDamageable
{
    public enum State {
        Chase,
        Scatter,
        Frightened,
        Eaten
    };

    public State CurrentState;

    public float speed;
    protected Movement movement;
    protected Pathfinding pathfinding;
    public LayerMask wallLayer;
    protected bool canMove;
    protected Pacman player;
    protected Vector2 targetPosition;
    protected Vector2 targetDirection;
    protected bool run;
    protected Vector2 spawnPosition;
    public Transform pointer;
    public Vector2 homePosition;

    private Vector2 teleportA = new Vector2(-14, 3);
    private Vector2 teleportB = new Vector2(15, 3);

    private void Awake() {
        movement = new Movement(speed);
        pathfinding = new Pathfinding(transform, wallLayer);
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Pacman>();
    }

    private void OnEnable() {
        Pacman.OnPlayerDamage += Reset;
        Pacman.OnPlayerDeath += Stop;
        Pacman.OnPlayerWin += Stop;
        GameManager.OnStateChange += ChangeState;
    }

    private void OnDisable() {
        Pacman.OnPlayerDamage -= Reset;
        Pacman.OnPlayerDeath -= Stop;
        Pacman.OnPlayerWin -= Stop;
        GameManager.OnStateChange -= ChangeState;
    }

    private void Start() {
        spawnPosition = transform.position;
        StartCoroutine(DelayStart());
    }

    protected virtual void Update() {
        if (run == false) {
            return;
        }

        if ((Vector2)transform.position + targetDirection == teleportA) {
            transform.position = teleportB;
            canMove = false;
        }
        else if ((Vector2)transform.position + targetDirection == teleportB) {
            transform.position = teleportA;
            canMove = false;
        }

        RaycastHit2D[] hitAll = Physics2D.RaycastAll(transform.position, targetDirection, .55f);
        foreach (RaycastHit2D hit in hitAll) {
            IDamageable damageableObject = hit.collider.GetComponent<IDamageable>();
            if (damageableObject != null) {
                //damageableObject.Death(gameObject.tag);
            }
        }

        if (canMove) {
            canMove = movement.Move(transform, targetPosition);
        } else {
            if (CurrentState == State.Scatter) {
                targetPosition = homePosition;
                pointer.position = targetPosition;
                targetDirection = pathfinding.GetShortestDirection(targetPosition, targetDirection);
                targetPosition = (Vector2)transform.position + targetDirection;
                canMove = true;
            }
        }
    }

    public void Death(string attacker) {
        if (attacker == "Player") {
            Debug.Log(name + " has been killed");
        }
    }

    private void Reset() {
        Stop();
        transform.position = spawnPosition;
        StartCoroutine(DelayStart());
    }

    private void Stop() {
        run = false;
        canMove = false;
    }

    private IEnumerator DelayStart() {
        yield return new WaitForSeconds(3);
        run = true;
    }

    private void ChangeState(string state) {
        switch (state) {
            case "chase":
                CurrentState = State.Chase;
                break;
            case "scatter":
                CurrentState = State.Scatter;
                break;
        }
    }
}
