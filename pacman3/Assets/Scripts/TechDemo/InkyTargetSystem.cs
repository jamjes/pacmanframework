using UnityEngine;

public class InkyTargetSystem : GhostController
{
    public Transform Blinky;

    protected override void Update() {
        if (canMove == false && CurrentState == State.Chase) {
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

        base.Update();
    }
}
