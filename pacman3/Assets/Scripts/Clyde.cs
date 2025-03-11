using UnityEngine;
using CustomVariables;

public class Clyde : Ghost
{
    protected override void OnEnable() {
        base.OnEnable();
    }

    protected override void OnDisable() {
        base.OnDisable();
    }
    protected override void Update() {
        if (canMove == false && CurrentState == GhostState.Chase) {
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
