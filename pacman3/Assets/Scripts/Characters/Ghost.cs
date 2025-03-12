using UnityEngine;
using System.Collections;
using CustomVariables;
using Unity.VisualScripting;

public class Ghost : MonoBehaviour, IDamageable
{
    private Color baseColor, frightenedColor = Color.blue;

    public GhostState CurrentState { get; private set; }
    private GhostState? SuperState = null;
    protected Movement movement;
    protected Pathfinding pathfinding;
    protected Pacman player;
    public Transform pointer;
    
    protected bool active;
    protected bool canMove;
    protected bool run;
    protected bool forceChase;
    private bool directionSwitch;
    
    [SerializeField] private float speed;

    protected Vector2 homePosition;
    public Vector2 targetDirection;
    protected Vector2 startingDirection;
    protected Vector2 targetPosition;
    protected Vector2 spawnPosition;
    private Vector2 startPosition = new Vector2(0, 3);
    public GridSettings gridSettings;

    private void Awake() {
        movement = new Movement(speed);
        pathfinding = new Pathfinding(transform, gridSettings.WallLayer, gridSettings.AllBlockingLayers);
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Pacman>();
    }

    protected virtual void OnEnable() {
        Pacman.OnPlayerDamage += ResetObject;
        Pacman.OnPlayerDeath += Stop;
        Pacman.OnPlayerWin += Stop;
        Pacman.OnPowerPellet += PowerPelletEvent;
    }

    protected virtual void OnDisable() {
        Pacman.OnPlayerDamage -= ResetObject;
        Pacman.OnPlayerDeath -= Stop;
        Pacman.OnPlayerWin -= Stop;
        Pacman.OnPowerPellet -= PowerPelletEvent;
    }

    protected virtual void Start() {
        spawnPosition = transform.position;
        baseColor = GetComponentInChildren<SpriteRenderer>().color;
        StartCoroutine(DelayStart());
    }

    private void Update() {
        if (run == false) {
            return;
        }

        if ((Vector2)transform.position + targetDirection == gridSettings.TeleportA) {
            transform.position = gridSettings.TeleportB;
            canMove = false;
        }
        else if ((Vector2)transform.position + targetDirection == gridSettings.TeleportB) {
            transform.position = gridSettings.TeleportA;
            canMove = false;
        }
        else if ((Vector2)transform.position == gridSettings.SlowZoneA && targetDirection == Vector2.left
                || (Vector2)transform.position == gridSettings.SlowZoneB && targetDirection == Vector2.right) {
            UpdateSpeed(5);
        }
        else if ((Vector2)transform.position == gridSettings.SlowZoneA && targetDirection == Vector2.right
                || (Vector2)transform.position == gridSettings.SlowZoneB && targetDirection == Vector2.left) {
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
    }

    protected void UpdateSpeed(float value) {
        speed = value;
        movement.SetMoveSpeed(speed);
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
        
        if (forceChase && targetState != GhostState.Frightened) {
            CurrentState = GhostState.Chase;
        } else {
            CurrentState = targetState;
            if (CurrentState == GhostState.Frightened) {
                directionSwitch = false;
            }
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
                if (SuperState != null) {
                    SetState((GhostState)SuperState);
                    targetDirection = Vector2.right;
                } else {
                    SetState(GhostState.Chase);
                }
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
        if (directionSwitch == false) {
            targetDirection *= -1;
            directionSwitch = true;
        }
        else {
            targetDirection = pathfinding.GetRandomDirection(targetDirection, gridSettings.AllBlockingLayers);
        }

        targetPosition = (Vector2)transform.position + targetDirection;
        canMove = true;
    }

    private void EatenUpdate() {
        Debug.Log("CurrentState = Eaten");
    }

    private void PowerPelletEvent() {
        StartCoroutine(PowerPelletDuration());
        SuperState = GhostState.Frightened;
        if (CurrentState == GhostState.Chase || CurrentState == GhostState.Scatter) {
            SetState(GhostState.Frightened);
        }
    }

    private IEnumerator PowerPelletDuration() {
        GetComponentInChildren<SpriteRenderer>().color = frightenedColor;
        yield return new WaitForSeconds(7);
        SuperState = null;
        if (CurrentState == GhostState.Frightened) {
            CurrentState = GhostState.Chase;
        }
        GetComponentInChildren<SpriteRenderer>().color = baseColor;
    }
}
