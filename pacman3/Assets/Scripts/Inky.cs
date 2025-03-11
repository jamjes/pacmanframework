using UnityEngine;
using CustomVariables;
public class Inky : Ghost
{
    public Transform Blinky;

    protected override void OnEnable() {
        base.OnEnable();
    }

    protected override void OnDisable() {
        base.OnDisable();
    }

    protected override void ChaseUpdate() {
        if (canMove == false && CurrentState == GhostState.Chase) {
            Vector2 pacmanPos;

            if (player.Direction == Vector2.up) {
                pacmanPos = (Vector2)player.transform.position + (Vector2.left + Vector2.up) * 2;
            }
            else {
                pacmanPos = (Vector2)player.transform.position + player.TargetDirection * 2;
            }

            float x = Blinky.position.x - pacmanPos.x;
            float y = Blinky.position.y - pacmanPos.y;

            targetPosition = new Vector2(x, y) * -1;

            pointer.position = targetPosition;
            targetDirection = pathfinding.GetShortestDirection(targetPosition, targetDirection);
            targetPosition = (Vector2)transform.position + targetDirection;
            canMove = true;
        }
    }
}
