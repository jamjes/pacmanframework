using UnityEngine;

public class Pinky : Ghost
{
    protected override void Update() {
        if (canMove == false && CurrentState == State.Chase) {
            if (player.Direction == Vector2.up) {
                targetPosition = (Vector2)player.transform.position + (Vector2.left + Vector2.up) * 2;
            } else {
                targetPosition = (Vector2)player.transform.position + player.TargetDirection * 2;
            }

            pointer.position = targetPosition;
            targetDirection = pathfinding.GetShortestDirection(targetPosition, targetDirection);
            targetPosition = (Vector2)transform.position + targetDirection;
            canMove = true;
        }

        base.Update();
    }
}
