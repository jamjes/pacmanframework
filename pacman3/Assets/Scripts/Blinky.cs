using UnityEngine;
using CustomVariables;

public class Blinky : Ghost
{
    protected override void OnEnable() {
        base.OnEnable();
        Pacman.OnAggroEnter += ForceChase;
    }

    protected override void OnDisable() {
        base.OnDisable();
        Pacman.OnAggroEnter -= ForceChase;
    }

    protected override void Update() {
        if (canMove == false && CurrentState == GhostState.Chase) {
            targetPosition = player.transform.position;
            pointer.position = targetPosition;
            targetDirection = pathfinding.GetShortestDirection(targetPosition, targetDirection);
            targetPosition = (Vector2)transform.position + targetDirection;
            canMove = true;
        }

        base.Update();
    }

    private void ForceChase() {
        forceChase = true;
        movement.SetMoveSpeed(8);
        SetState(GhostState.Chase);
    }
}
