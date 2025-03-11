using UnityEngine;
using System.Collections;
using CustomVariables;
using UnityEditor.Experimental.GraphView;

public class Ghost : MonoBehaviour, IDamageable
{
    Vector2 initPos = new Vector2(0,3);
    public GhostState CurrentState;
    public float speed;
    protected Movement movement;
    protected Pathfinding pathfinding;
    public LayerMask wallLayer, allLayers;
    protected bool canMove;
    protected Pacman player;
    protected Vector2 targetPosition;
    public Vector2 targetDirection;
    public Vector2 startingDirection;
    protected bool run;
    protected Vector2 spawnPosition;
    public Transform pointer;
    public Vector2 homePosition;
    protected bool forceChase;

    private Vector2 teleportA = new Vector2(-14, 0);
    private Vector2 teleportB = new Vector2(15, 0);
    private Vector2 startPosition = new Vector2(0, 3);
    private Vector2 slowZoneA = new Vector2(-8,0);
    private Vector2 slowZoneB = new Vector2(9,0);
    public bool active;

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

    private void Update() {
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
        else if ((Vector2)transform.position == slowZoneA && targetDirection == Vector2.left
                || (Vector2)transform.position == slowZoneB && targetDirection == Vector2.right) {
            UpdateSpeed(5);
        }
        else if ((Vector2)transform.position == slowZoneA && targetDirection == Vector2.right
                || (Vector2)transform.position == slowZoneB && targetDirection == Vector2.left) {
            if (forceChase) {
                UpdateSpeed(8);
            }
            else {
                UpdateSpeed(7);
            }
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
            return;
        }

        switch (CurrentState) {
            case GhostState.Disable:
                DisableUpdate();
                break;

            case GhostState.Chase:
                ChaseUpdate();
                break;

            case GhostState.Eaten:
                EatenUpdate();
                break;

            case GhostState.Scatter:
                ScatterUpdate();
                break;

            case GhostState.Frightened:
                FrightenedUpdate();
                break;
        }

        return;

        // --- //

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
                    if (active && ((Vector2)transform.position != startPosition)) {
                        targetPosition = startPosition;
                        pointer.position = targetPosition;
                        targetDirection = pathfinding.GetShortestDirectionIgnore(targetPosition, targetDirection);
                        targetPosition = (Vector2)transform.position + targetDirection;
                        canMove = true;
                    } else if (active && ((Vector2)transform.position == startPosition)) {
                        if (startPosition == Vector2.zero) {
                            startPosition = new Vector2(0, 3);
                        }
                        else {
                            SetState(GhostState.Chase);
                            if (forceChase) {
                                movement.SetMoveSpeed(8);
                            }
                            else {
                                movement.SetMoveSpeed(7);
                            }
                        }
                    }
                }
            }
        }
    }

    private void UpdateSpeed(float value) {
        speed = value;
        movement.SetMoveSpeed(speed);
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
            active = false;
        }
        else {
            SetState(GhostState.Scatter);
            active = true;
        }
        UpdateSpeed(7);
        StartCoroutine(DelayStart());
    }

    private void Stop() {
        run = false;
        canMove = false;
        targetDirection = Vector2.zero;
    }

    private IEnumerator DelayStart() {
        yield return new WaitForSeconds(3);
        run = true;
    }

    public void SetState(GhostState targetState) {
        if (active == false) {
            return;
        }
        
        if (forceChase) {
            CurrentState = GhostState.Chase;
        } else {
            CurrentState = targetState;
        }
    }

    public void Enable() {
        active = true;
        UpdateSpeed(5);
    }

    private void DisableUpdate() {
        if (active) {
            if ((Vector2)transform.position == startPosition) {
                UpdateSpeed(7);
                SetState(GhostState.Chase);
                return;
            }

            targetPosition = startPosition;
            targetDirection = pathfinding.GetShortestDirectionIgnore(targetPosition, targetDirection);
            targetPosition = (Vector2)transform.position + targetDirection;
            canMove = true;
        } else {
            if (targetDirection == Vector2.zero) {
                targetDirection = startingDirection;
            }

            if ((Vector2)transform.position == spawnPosition + Vector2.up) {
                targetDirection = Vector2.down;
            } else if ((Vector2)transform.position == spawnPosition + Vector2.down) {
                targetDirection = Vector2.up;
            }

            targetPosition = (Vector2)transform.position + targetDirection;
            canMove = true;
        }
    }

    protected virtual void ChaseUpdate() { }

    private void ScatterUpdate() {
        if (forceChase) {
            ChaseUpdate();
        }
        targetPosition = homePosition;
        targetDirection = pathfinding.GetShortestDirection(targetPosition, targetDirection);
        targetPosition = (Vector2)transform.position + targetDirection;
        canMove = true;
    }

    private void FrightenedUpdate() {
        Debug.Log("CurrentState = Frightened");
    }

    private void EatenUpdate() {
        Debug.Log("CurrentState = Eaten");
    }
}
