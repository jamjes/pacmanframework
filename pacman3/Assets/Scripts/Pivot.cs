using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pivot : MonoBehaviour
{
    public PacmanController pacman;

    void Update()
    {
        switch(pacman.TargetDirection) {
            case Vector2 dir when dir == Vector2.up:
            transform.rotation = Quaternion.Euler(0, 0, 90);
            break;

            case Vector2 dir when dir == Vector2.right:
                transform.rotation = Quaternion.Euler(0, 0, 0);
                break;

            case Vector2 dir when dir == Vector2.down:
                transform.rotation = Quaternion.Euler(0, 0, -90);
                break;

            case Vector2 dir when dir == Vector2.left:
                transform.rotation = Quaternion.Euler(0, 0, 180);
                break;
        }
    }
}
