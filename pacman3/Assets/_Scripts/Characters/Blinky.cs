using UnityEngine;

public class Blinky : Ghost
{
    protected override void Start() {
        homePosition = new Vector2(12, 17);
        active = true;
        SetState(CustomVariables.GhostState.Scatter);
        base.Start();
    }

    protected override void OnEnable() {
        base.OnEnable();
        Pacman.OnAggroEnter += ForceChase;
    }

    protected override void OnDisable() {
        base.OnDisable();
        Pacman.OnAggroEnter -= ForceChase;
    }

    protected override void ChaseUpdate() {
        targetPosition = player.transform.position;
        pointer.position = targetPosition;
        targetDirection = pathfinding.GetShortestDirection(targetPosition, targetDirection);
        targetPosition = (Vector2)transform.position + targetDirection;
        canMove = true;
    }

    private void ForceChase() {
        forceChase = true;
        UpdateSpeed(8);
        SetState(CustomVariables.GhostState.Chase);
    }
}
