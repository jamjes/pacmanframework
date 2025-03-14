using UnityEngine;

public class GridSettings : MonoBehaviour
{
    public Vector2 TeleportA = new Vector2(-14, 0);
    public Vector2 TeleportB = new Vector2(15, 0);
    public Vector2 SlowZoneA = new Vector2(-8, 0);
    public Vector2 SlowZoneB = new Vector2(9, 0);

    public LayerMask AllBlockingLayers;
    public LayerMask WallLayer;
}
