using UnityEngine;

public class Clyde : Ghost
{
    protected override void Update() {
        if (canMove == false && CurrentState == State.Chase) {
            float cost = pathfinding.CalculateHCost(transform.position, player.transform.position);

            if (cost >= 8) {
                targetPosition = player.transform.position;
            } else {
                targetPosition = homePosition;
            }
            
            pointer.position = targetPosition;
            targetDirection = pathfinding.GetShortestDirection(targetPosition, targetDirection);
            targetPosition = (Vector2)transform.position + targetDirection;
            canMove = true;
        }

        base.Update();
    }
}
