using UnityEngine;
using CustomVariables;

public class Pinky : Ghost
{
    protected override void OnEnable() {
        base.OnEnable();
    }

    protected override void OnDisable() {
        base.OnDisable();
    }

    protected override void Update() {
        if (canMove == false && CurrentState == GhostState.Chase) {
            if (player.TargetDirection == Vector2.up) {
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
