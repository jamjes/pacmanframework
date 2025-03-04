using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GhostParent : MonoBehaviour
{
    protected bool run = true;
    protected Vector2 spawnPosition;
    protected bool? isMoving = false;
    public SpriteRenderer Eyes;
    public Sprite upEyes, leftEyes, downEyes, rightEyes;
    public Animator anim;
    protected GameObject player;
    public LayerMask wallLayer;
    protected Vector2 targetPosition;
    protected Vector2 startPosition;
    protected Vector2 currentDirection;
    protected float elapsedTime;
    public float duration = 1f;

    public Transform target;

    private void OnEnable() {
        Player.OnPlayerDeath += Pause;
        Player.OnPlayerReset += Play;
    }

    private void OnDisable() {
        Player.OnPlayerDeath -= Pause;
        Player.OnPlayerReset -= Play;
    }

    private void Pause() {
        run = false;
        anim.speed = 0;
    }

    private void Play() {
        isMoving = false;
        transform.position = spawnPosition;
        run = true;
        anim.speed = 1;
    }

    protected float CalculateHCost(Vector2 startPos, Vector2 targetPos) {
        float x = targetPos.x - startPos.x;
        float y = targetPos.y - startPos.y;
        return Mathf.Abs(x) + Mathf.Abs(y);
    }

    protected Vector2[] GetAvailableDirections(Vector2 currentDirection, LayerMask wallLayer) {
        List<Vector2> directions = new List<Vector2>();

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, .55f, wallLayer);
        if (hit.collider == null && currentDirection * -1 != Vector2.right) {
            directions.Add(Vector2.right);
        }

        hit = Physics2D.Raycast(transform.position, Vector2.down, .55f, wallLayer);
        if (hit.collider == null && currentDirection * -1 != Vector2.down) {
            directions.Add(Vector2.down);
        }

        hit = Physics2D.Raycast(transform.position, Vector2.left, .55f, wallLayer);
        if (hit.collider == null && currentDirection * -1 != Vector2.left) {
            directions.Add(Vector2.left);
        }

        hit = Physics2D.Raycast(transform.position, Vector2.up, .55f, wallLayer);
        if (hit.collider == null && currentDirection * -1 != Vector2.up) {
            directions.Add(Vector2.up);
        }

        return directions.ToArray();
    }

    protected Vector2 GetShortestDirection(Vector2 targetPosition, Vector2 currentDirection, LayerMask wallLayer) {
        Vector2[] availableDirections = GetAvailableDirections(currentDirection, wallLayer);
        float[] costs = new float[availableDirections.Length];
        int i = 0;
        foreach (Vector2 direction in availableDirections) {
            costs[i] = CalculateHCost((Vector2)transform.position + direction, targetPosition);
            i++;
        }

        for (int a = 0; a < costs.Length - 1; a++) {
            for (int b = 0; b < costs.Length - a - 1; b++) {
                bool swap = false;
                if (costs[b] > costs[b + 1]) {
                    swap = true;
                }
                else if (costs[b] == costs[b + 1]) {
                    if (availableDirections[b].y < availableDirections[b + 1].y) {
                        swap = true;
                    }
                    else if (availableDirections[b].x > availableDirections[b + 1].x) {
                        swap = true;
                    }
                }

                if (swap) {
                    float temp = costs[b];
                    costs[b] = costs[b + 1];
                    costs[b + 1] = temp;

                    Vector2 tempV = availableDirections[b];
                    availableDirections[b] = availableDirections[b + 1];
                    availableDirections[b + 1] = tempV;
                }
            }
        }

        return availableDirections[0];
    }

    protected void UpdateEyes(Vector2 direction) {
        switch(direction) {
            case Vector2 d when d == Vector2.up:
                Eyes.sprite = upEyes; break;
            case Vector2 d when d == Vector2.left:
                Eyes.sprite = leftEyes; break;
            case Vector2 d when d == Vector2.down:
                Eyes.sprite = downEyes; break;
            case Vector2 d when d == Vector2.right:
                Eyes.sprite = rightEyes; break;
        }
    }
}
