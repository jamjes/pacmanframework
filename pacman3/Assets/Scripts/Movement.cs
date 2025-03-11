using UnityEngine;

public class Movement {
    private float speed;

    public Movement(float speed) {
        this.speed = speed;
    }

    public bool Move(Transform self, Vector2 targetPosition) {
        self.position = Vector2.MoveTowards(self.position, targetPosition, speed * Time.deltaTime);
        if ((Vector2)self.position == targetPosition) {
            return false;
        } else {
            return true;
        }
    }

    public void SetMoveSpeed(float speed) {
        this.speed = speed;
    }
}
