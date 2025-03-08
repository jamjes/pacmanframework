using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamageable
{
    public Vector2 TargetDirection { get; private set; }
    public Vector2 Direction { get; private set; }
    private Vector2 targetPosition;
    public LayerMask wallLayer;
    public float speed;
    private bool canMove;
    private Movement movement;
    private bool run;
    private Vector2 spawnPosition;
    private int lives = 3;
    public delegate void PlayerEvent();
    public static event PlayerEvent OnPlayerDamage;
    public static event PlayerEvent OnPlayerDeath;
    public static event PlayerEvent OnPlayerWin;
    private int score;
    private int totalPellets;
    private Vector2 teleportA = new Vector2(-14, 3);
    private Vector2 teleportB = new Vector2(15, 3);

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
            if ((Vector2)transform.position + Direction == teleportA) {
                transform.position = teleportB;
            } else if ((Vector2)transform.position + Direction == teleportB) {
                transform.position = teleportA;
            }

            RaycastHit2D hit = Physics2D.Raycast(transform.position, TargetDirection, .55f, wallLayer);

            if (hit.collider == null) {
                Direction = TargetDirection;
            } else {
                hit = Physics2D.Raycast(transform.position, Direction, .55f, wallLayer);
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
        if (attacker == "Ghost") {
            lives--;
            Debug.Log(name + " has been killed");
            run = false;
            canMove = false;
            if (lives > 0) {
                if (OnPlayerDamage != null) {
                    transform.position = spawnPosition;
                    OnPlayerDamage();
                    StartCoroutine(DelayStart());
                }
            } else {
                if (OnPlayerDeath != null) {
                    OnPlayerDeath();
                }
            }
            
        }
    }

    private IEnumerator DelayStart() {
        yield return new WaitForSeconds(3);
        run = true;
    }

    private void CollisionCheck(Collider2D collision) {
        if (collision.tag == "Pellet") {
            score+=10;
            totalPellets--;
            Destroy(collision.gameObject);
            if (totalPellets == 0) {
                if (OnPlayerWin != null) {
                    OnPlayerWin();
                    run = false;
                }
            }
        } else if (collision.tag == "Power Pellet") {
            score += 50;
            totalPellets--;
            Destroy(collision.gameObject);
            if (totalPellets == 0) {
                if (OnPlayerWin != null) {
                    OnPlayerWin();
                    run = false;
                }
            }
        }
    }
}
