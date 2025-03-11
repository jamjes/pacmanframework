using UnityEngine;
using System.Collections;
using CustomVariables;

public class Ghost : MonoBehaviour, IDamageable
{
    Vector2 initPos = new Vector2(0,3);
    public Color startColor, frightenedColor;
    public GhostState CurrentState;
    public float speed;
    protected Movement movement;
    protected Pathfinding pathfinding;
    public LayerMask wallLayer, allLayers;
    protected bool canMove;
    protected Pacman player;
    protected Vector2 targetPosition;
    public Vector2 targetDirection;
    protected bool run;
    protected Vector2 spawnPosition;
    public Transform pointer;
    public Vector2 homePosition;
    protected bool forceChase;

    private Vector2 teleportA = new Vector2(-14, 0);
    private Vector2 teleportB = new Vector2(15, 0);
    private Vector2 startPosition = new Vector2(0, 3);
    public bool isEnabled;

    private void Awake() {
        movement = new Movement(speed);
        pathfinding = new Pathfinding(transform, wallLayer, allLayers);
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Pacman>();
    }

    protected virtual void OnEnable() {
        Pacman.OnPlayerDamage += ResetObject;
        Pacman.OnPlayerDeath += Stop;
        Pacman.OnPlayerWin += Stop;
    }

    protected virtual void OnDisable() {
        Pacman.OnPlayerDamage -= ResetObject;
        Pacman.OnPlayerDeath -= Stop;
        Pacman.OnPlayerWin -= Stop;
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
                damageableObject.Death(gameObject.tag);
            }
        }

        if (canMove) {
            canMove = movement.Move(transform, targetPosition);
        } else {
            if (CurrentState == GhostState.Scatter) {
                targetPosition = homePosition;
                pointer.position = targetPosition;
                targetDirection = pathfinding.GetShortestDirection(targetPosition, targetDirection);
                targetPosition = (Vector2)transform.position + targetDirection;
                canMove = true;
            } else {
                if (CurrentState == GhostState.Disable) {
                    if (isEnabled && ((Vector2)transform.position != startPosition)) {
                        targetPosition = startPosition;
                        pointer.position = targetPosition;
                        targetDirection = pathfinding.GetShortestDirectionIgnore(targetPosition, targetDirection);
                        targetPosition = (Vector2)transform.position + targetDirection;
                        canMove = true;
                    } else if (isEnabled && ((Vector2)transform.position == startPosition)) {
                        SetState(GhostState.Chase);
                    }
                }
            }
        }
    }

    private void InitUpdate() {
        if ((Vector2)transform.position == initPos) {
            SetState(GhostState.Chase);
            return;
        }

        targetPosition = initPos;
        targetDirection = pathfinding.GetShortestDirectionIgnore(targetPosition, targetDirection);
        targetPosition = (Vector2)transform.position + targetDirection;
        canMove = true;
    }

    private void IdleUpdate() {
        if ((Vector2)transform.position == spawnPosition + Vector2.up) {
            targetDirection = Vector2.down;
        } 
        else if ((Vector2)transform.position == spawnPosition + Vector2.down) {
            targetDirection = Vector2.up;
        }

        targetPosition = (Vector2)transform.position + targetDirection;
        canMove = true;
    }

    public void Death(string attacker) {
        if (attacker == "Player") {
            Debug.Log(name + " has been killed");
        }
    }

    private void ResetObject() {
        Stop();
        transform.position = spawnPosition;
        if (spawnPosition != startPosition) {
            SetState(GhostState.Disable);
            isEnabled = false;
        } else {
            SetState(GhostState.Scatter);
        }
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

    public void SetState(GhostState targetState) {
        if (isEnabled == false) {
            return;
        }
        
        if (forceChase) {
            CurrentState = GhostState.Chase;
        } else {
            CurrentState = targetState;
        }
    }

    public void Enable() {
        isEnabled = true;
    }
}
