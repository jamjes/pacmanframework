using UnityEngine;

public class Pinky : Ghost
{
    protected override void Start() {
        homePosition = new Vector2(-11, 17);
        startingDirection = Vector2.down;
        base.Start();
    }

    protected override void ChaseUpdate() {
        if (player.TargetDirection == Vector2.up) {
            targetPosition = (Vector2)player.transform.position + (Vector2.left + Vector2.up) * 2;
        }
        else {
            targetPosition = (Vector2)player.transform.position + player.TargetDirection * 2;
        }

        pointer.position = targetPosition;
        targetDirection = pathfinding.GetShortestDirection(targetPosition, targetDirection);
        targetPosition = (Vector2)transform.position + targetDirection;
        canMove = true;
    }
}
