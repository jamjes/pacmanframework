using UnityEngine;

public class CharacterMovement
{
    private float elapsedTime;
    private float duration;

    public CharacterMovement(float duration) {
        this.elapsedTime = 0;
        this.duration = duration;
    }

    public bool MoveTo(Transform self, Vector2 startPosition, Vector2 endPosition) {
        elapsedTime += Time.deltaTime;
        float percentageComplete = elapsedTime / duration;
        if (elapsedTime > duration) {
            percentageComplete = 1;
        }

        self.position = Vector2.Lerp(startPosition, endPosition, percentageComplete);

        if (percentageComplete == 1) {
            elapsedTime = 0;
            return true;
        } else {
            return false;
        }
    }
}
