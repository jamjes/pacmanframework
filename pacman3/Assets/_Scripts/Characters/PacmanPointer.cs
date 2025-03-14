using UnityEngine;

public class PacmanPointer : MonoBehaviour
{
    public Pacman player;
    public SpriteRenderer spr;

    private void Update() {
        if (player.TargetDirection == Vector2.zero) {
            spr.enabled = false;
        } else {
            spr.enabled = true;
        }
        
        switch(player.TargetDirection) {
            case Vector2 dir when dir == Vector2.up:
                transform.rotation = Quaternion.Euler(0, 0, 90); break;
            case Vector2 dir when dir == Vector2.left:
                transform.rotation = Quaternion.Euler(0, 0, 180); break;
            case Vector2 dir when dir == Vector2.down:
                transform.rotation = Quaternion.Euler(0, 0, -90); break;
            case Vector2 dir when dir == Vector2.right:
                transform.rotation = Quaternion.Euler(0, 0, 0); break;
        }
    }
}
