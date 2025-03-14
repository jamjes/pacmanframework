using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Pacman : MonoBehaviour, IDamageable
{
    public Vector2 TargetDirection { get; private set; }
    public Vector2 Direction { get; private set; }
    private Vector2 targetPosition;
    [SerializeField] private float speed;
    [SerializeField] private int score;
    private bool canMove;
    private Movement movement;
    private bool run;
    private Vector2 spawnPosition;
    private int lives = 3;
    public delegate void PlayerEvent();
    public static event PlayerEvent OnPlayerDamage;
    public static event PlayerEvent OnPlayerDeath;
    public static event PlayerEvent OnPlayerWin;
    public static event PlayerEvent OnAggroEnter;
    public static event PlayerEvent OnPowerPellet;
    public delegate void UIEvent(float value);
    public static event UIEvent OnScoreUpdate;
    private int totalPellets;
    public GridSettings gridSettings;

    int scoreMultiplier = 1;

    private void Awake() {
        movement = new Movement(speed);
    }

    private void Start() {
        TargetDirection = Vector2.left;
        spawnPosition = transform.position;
        totalPellets = GameObject.FindGameObjectsWithTag("Pellet").Length + GameObject.FindGameObjectsWithTag("Power Pellet").Length;
        StartCoroutine(DelayStart());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            TargetDirection = Vector2.up;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            TargetDirection = Vector2.left;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow)) {
            TargetDirection = Vector2.down;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow)) {
            TargetDirection = Vector2.right;
        }

        if (run == false) {
            return;
        }

        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Direction, .55f);
        foreach(RaycastHit2D hit in hits) {
            CollisionCheck(hit.collider);
        }

        if (canMove == false) {
            if ((Vector2)transform.position + Direction == gridSettings.TeleportA) {
                transform.position = gridSettings.TeleportB;
            } else if ((Vector2)transform.position + Direction == gridSettings.TeleportB) {
                transform.position = gridSettings.TeleportA;
            }

            RaycastHit2D hit = Physics2D.Raycast(transform.position, TargetDirection, .55f, gridSettings.AllBlockingLayers);

            if (hit.collider == null) {
                Direction = TargetDirection;
            } else {
                hit = Physics2D.Raycast(transform.position, Direction, .55f, gridSettings.AllBlockingLayers);
                if (hit.collider != null) {
                    Direction = Vector2.zero;
                }
            }

            if (Direction != Vector2.zero) {
                targetPosition = (Vector2)transform.position + Direction;
                canMove = true;
            }
        }
        
        if (canMove) {
            canMove = movement.Move(transform, targetPosition);
        }
    }

    public void Death(string attacker) {
        if (attacker == "Ghost" && run == true) {
            run = false;
            lives--;
            Direction = Vector2.zero;
            TargetDirection = Vector2.zero;
            if (lives == 0) {
                if (OnPlayerDeath != null) {
                    OnPlayerDeath();
                }
            } else {
                if (OnPlayerDamage != null) {
                    OnPlayerDamage();
                }
                StartCoroutine(ResetAfterDelay());
            }
        }
    }

    private IEnumerator ResetAfterDelay() {
        yield return new WaitForSeconds(3f);
        TargetDirection = Vector2.left;
        transform.position = spawnPosition;
        canMove = false;
        yield return new WaitForSeconds(3f);
        run = true;
    }

    private IEnumerator DelayStart() {
        yield return new WaitForSeconds(3);
        run = true;
    }

    private IEnumerator DamageDelay() {
        yield return new WaitForSeconds(3);
        if (lives > 0) {
            if (OnPlayerDamage != null) {
                OnPlayerDamage();
            }
            transform.position = spawnPosition;
            StartCoroutine(DelayStart());
        }
        else {
            if (OnPlayerDeath != null) {
                OnPlayerDeath();
            }
        }
    }

    private void CollisionCheck(Collider2D collision) {
        if (collision.tag == "Pellet" || collision.tag == "Power Pellet") {
            if (collision.tag == "Pellet") {
                score += 10;
            }
            else {
                score += 50;
                scoreMultiplier = 1;
                if (OnPowerPellet != null) {
                    OnPowerPellet();
                }
            }
            if (OnScoreUpdate != null) {
                OnScoreUpdate(score);
            }
            totalPellets--;
            Destroy(collision.gameObject);
            if (totalPellets == 0) {
                run = false;
                if (OnPlayerWin != null) {
                    OnPlayerWin();
                }
            }
            else if (totalPellets == 20) {
                if (OnAggroEnter != null) {
                    OnAggroEnter();
                }
            }
        }
        else if (collision.tag == "Ghost") {
            if (collision.GetComponent<Ghost>().CurrentState == CustomVariables.GhostState.Frightened) {
                IDamageable damageableObject = collision.GetComponent<IDamageable>();
                if (damageableObject != null) {
                    damageableObject.Death(gameObject.tag);
                    score += (200 * scoreMultiplier);
                    scoreMultiplier *= 2;
                }
            }
        }
    }
}
