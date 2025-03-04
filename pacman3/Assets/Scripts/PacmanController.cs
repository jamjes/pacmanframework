using System.Linq.Expressions;
using UnityEngine;

public class PacmanController : MonoBehaviour
{
    [SerializeField][Tooltip("The time it takes this character to complete 1 step")] private float duration;
    public Vector2 TargetDirection { get; private set; }
    private Vector2 previousDirection;
    private CharacterMovement movement;
    public LayerMask wallLayer;
    private Vector2 startPosition, targetPosition;
    private bool move;

    private void Awake() {
        movement = new CharacterMovement(duration);
    }

    private void Update() {
        ListenForInputs();
        
        if (move == false) {
            Vector2 direction = CalculateDirection();

            if (direction != Vector2.zero) {
                startPosition = transform.position;
                targetPosition = startPosition + direction;
                previousDirection = direction;
                move = true;
            }
        } else {
            move = !movement.MoveTo(transform, startPosition, targetPosition);
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
        else if (previousDirection != Vector2.zero) {
            hit = Physics2D.Raycast(transform.position, previousDirection, .55f, wallLayer);
            if (hit.collider == null) { //Continuing in previous direction is not a wall
                direction = previousDirection;
            }
        }

        return direction;
    }
}
