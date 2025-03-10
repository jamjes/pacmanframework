using UnityEngine;

public class Blinky : Ghost
{
    protected override void Update() {
        if (canMove == false && CurrentState == State.Chase) {
            targetPosition = player.transform.position;
            pointer.position = targetPosition;
            targetDirection = pathfinding.GetShortestDirection(targetPosition, targetDirection);
            targetPosition = (Vector2)transform.position + targetDirection;
            canMove = true;
        }

        base.Update();
    }
}
