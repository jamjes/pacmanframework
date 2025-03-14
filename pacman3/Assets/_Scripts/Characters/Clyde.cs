using UnityEngine;

public class Clyde : Ghost
{
    protected override void Start() {
        homePosition = new Vector2(12, -16);
        startingDirection = Vector2.up;
        base.Start();
    }

    protected override void ChaseUpdate() {
        float cost = pathfinding.CalculateHCost(transform.position, player.transform.position);

        if (cost >= 8) {
            targetPosition = player.transform.position;
        }
        else {
            targetPosition = homePosition;
        }

        pointer.position = targetPosition;
        targetDirection = pathfinding.GetShortestDirection(targetPosition, targetDirection);
        targetPosition = (Vector2)transform.position + targetDirection;
        canMove = true;
    }
}
