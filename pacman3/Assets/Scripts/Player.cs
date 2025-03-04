using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    bool LEFT, RIGHT, UP, DOWN;
    public LayerMask groundLayer;
    bool isMoving = false;
    Vector2 direction;
    public Vector2 TargetDirection {  get; private set; }
    Vector2 currentPosition, targetPosition, startPosition;
    float elapsedTime;
    public float duration;
    public GameObject pivot;
    public Animator anim;
    public bool dead;
    private static readonly int deathAnim = Animator.StringToHash("death");
    private static readonly int eatAnim = Animator.StringToHash("eat");
    public int lives = 3;
    public int score;

    public delegate void PlayerEvent();
    public static event PlayerEvent OnPlayerDeath;
    public static event PlayerEvent OnPlayerReset;

    public delegate void UIEvent(int score);
    public static event UIEvent OnScoreUpdate;

    private void Start() {
        startPosition = transform.position;
        TargetDirection = Vector2.right;
    }

    private void Update() {
        if (dead) {
            return;
        }
        else {
            RaycastHit2D[] collisions = Physics2D.CircleCastAll(transform.position, .5f, Vector2.zero);
            foreach (RaycastHit2D obj in collisions) {
                if (obj.collider.tag == "Ghost") {
                    StartCoroutine(Respawn());
                    return;
                } else if (obj.collider.tag == "Pellet") {
                    Destroy(obj.collider.gameObject);
                    score+=10;
                    if (OnScoreUpdate != null) {
                        OnScoreUpdate(score);
                    }
                } else if (obj.collider.tag == "Power Pellet") {
                    Destroy(obj.collider.gameObject);
                    score += 50;
                    if (OnScoreUpdate != null) {
                        OnScoreUpdate(score);
                    }
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            TargetDirection = Vector2.right;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow)) {
            TargetDirection = Vector2.down;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            TargetDirection = Vector2.left;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow)) {
            TargetDirection = Vector2.up;
        }

        switch (direction) {
            case Vector2 dir when dir == Vector2.up:
                transform.rotation = Quaternion.Euler(0, 0, 90);
                break;

            case Vector2 dir when dir == Vector2.right:
                transform.rotation = Quaternion.Euler(0, 0, 0);
                break;

            case Vector2 dir when dir == Vector2.down:
                transform.rotation = Quaternion.Euler(0, 0, -90);
                break;

            case Vector2 dir when dir == Vector2.left:
                transform.rotation = Quaternion.Euler(0, 0, 180);
                break;
        }

        if (direction == Vector2.zero) {
            anim.speed = 0;
        }
        else {
            anim.speed = 1;
        }

        if (isMoving) {
            elapsedTime += Time.deltaTime;
            float percentageComplete = elapsedTime / duration;
            if (elapsedTime > duration) {
                percentageComplete = 1;
            }
            transform.position = Vector2.Lerp(currentPosition, targetPosition, percentageComplete);
            if (percentageComplete == 1) {
                elapsedTime = 0;
                isMoving = false;
            }
        }
        else {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left, .55f, groundLayer);
            LEFT = hit.collider != null;

            hit = Physics2D.Raycast(transform.position, Vector2.right, .55f, groundLayer);
            RIGHT = hit.collider != null;

            hit = Physics2D.Raycast(transform.position, Vector2.up, .55f, groundLayer);
            UP = hit.collider != null;

            hit = Physics2D.Raycast(transform.position, Vector2.down, .55f, groundLayer);
            DOWN = hit.collider != null;

            switch (TargetDirection) {
                case Vector2 dir when dir == Vector2.up:
                    if (!UP) {
                        direction = Vector2.up;
                    }
                    break;

                case Vector2 dir when dir == Vector2.right:
                    if (!RIGHT) {
                        direction = Vector2.right;
                    }
                    break;

                case Vector2 dir when dir == Vector2.down:
                    if (!DOWN) {
                        direction = Vector2.down;
                    }
                    break;

                case Vector2 dir when dir == Vector2.left:
                    if (!LEFT) {
                        direction = Vector2.left;
                    }
                    break;
            }

            switch (direction) {
                case Vector2 dir when dir == Vector2.up:
                    if (UP) {
                        direction = Vector2.zero;
                    }
                    break;

                case Vector2 dir when dir == Vector2.right:
                    if (RIGHT) {
                        direction = Vector2.zero;
                    }
                    break;

                case Vector2 dir when dir == Vector2.down:
                    if (DOWN) {
                        direction = Vector2.zero;
                    }
                    break;

                case Vector2 dir when dir == Vector2.left:
                    if (LEFT) {
                        direction = Vector2.zero;
                    }
                    break;
            }

            Vector2 posA = new Vector2(14, 1);
            Vector2 posB = new Vector2(-13, 1);

            if ((Vector2)transform.position == posA) {
                currentPosition = posB;
                direction = Vector2.right;
                targetPosition = posB + direction;
                isMoving = true;

            }
            else if ((Vector2)transform.position == posB) {
                currentPosition = posA;
                direction = Vector2.left;
                targetPosition = posA + direction;
                isMoving = true;
            }
            else {
                if (direction != Vector2.zero) {
                    currentPosition = transform.position;
                    targetPosition = currentPosition + direction;
                    isMoving = true;
                }
            }

            
        }
    }

    private IEnumerator Respawn() {
        dead = true;
        if (OnPlayerDeath != null) {
            OnPlayerDeath();
        }
        lives--;
        anim.speed = 0;
        yield return new WaitForSeconds(.3f);
        transform.rotation = Quaternion.Euler(0, 0, 0);
        anim.CrossFade(deathAnim, 0, 0);
        anim.speed = 1;
        yield return new WaitForSeconds(1.5f);
        if (lives > 0) {
            transform.position = startPosition;
            targetPosition = transform.position;
            currentPosition = transform.position;
            TargetDirection = Vector2.zero;
            direction = Vector2.zero;
            anim.CrossFade(eatAnim, 0, 0);
            if (OnPlayerReset != null) {
                OnPlayerReset();
            }
            yield return new WaitForSeconds(.1f);
            dead = false;
            
        } else {
            Debug.Log("Game OVER");
        }
    }
}
