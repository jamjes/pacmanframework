using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pink : GhostParent
{
    Vector2 playerDirection;

    private void Awake() {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Start() {
        spawnPosition = transform.position;
    }

    private void Update() {
        if (run == false) {
            return;
        }

        if (isMoving == false) {
            Vector2 posA = new Vector2(14,1);
            Vector2 posB = new Vector2(-13, 1);

            playerDirection = player.GetComponent<Player>().TargetDirection;
            Vector2 offset;
            if (playerDirection == Vector2.up) {
                offset = new Vector2(-2, 2);
            }
            else {
                offset = playerDirection * 2;
            }
            Vector2 endPosition = new Vector2((int)player.transform.position.x, (int)player.transform.position.y) + offset;

            target.position = endPosition;

            if ((Vector2)transform.position == posA) {
                transform.position = posB;
                currentDirection = Vector2.right;
                targetPosition = posB + currentDirection;

            } 
            else if ((Vector2)transform.position == posB) {
                transform.position = posA;
                currentDirection = Vector2.left;
                targetPosition = posA + currentDirection;
            }
            else {
                currentDirection = GetShortestDirection(endPosition, currentDirection, wallLayer);
                targetPosition = (Vector2)transform.position + currentDirection;
            }
            
            startPosition = transform.position;
            isMoving = true;
            elapsedTime = 0;
            UpdateEyes(currentDirection);
        }

        if (isMoving == null) {
            if (Input.GetKeyDown(KeyCode.Return)) {
                isMoving = true;
            }
        }

        if (isMoving == true) {
            elapsedTime += Time.deltaTime;
            float percentageComplete = elapsedTime / duration;
            if (elapsedTime > duration) {
                percentageComplete = 1;
            }
            transform.position = Vector2.Lerp(startPosition, targetPosition, percentageComplete);
            if (percentageComplete == 1) {
                isMoving = false;
            }
        }
    }
}
