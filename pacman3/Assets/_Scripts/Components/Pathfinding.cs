using System.Collections.Generic;
using UnityEngine;

public class Pathfinding
{
    private Transform self;
    private LayerMask wallLayer;
    private LayerMask allWalls;

    public Pathfinding(Transform self, LayerMask wallLayer, LayerMask allWalls) { 
        this.self = self;
        this.wallLayer = wallLayer;
        this.allWalls = allWalls;
    }

    public float CalculateHCost(Vector2 startPos, Vector2 targetPos) {
        float x = targetPos.x - startPos.x;
        float y = targetPos.y - startPos.y;
        return Mathf.Abs(x) + Mathf.Abs(y);
    }

    public Vector2[] GetAvailableDirections(Vector2 currentDirection, LayerMask targetLayer) {
        List<Vector2> directions = new List<Vector2>();

        RaycastHit2D hit = Physics2D.Raycast(self.position, Vector2.right, .55f, targetLayer);
        if (hit.collider == null && currentDirection * -1 != Vector2.right) {
            directions.Add(Vector2.right);
        }

        hit = Physics2D.Raycast(self.position, Vector2.down, .55f, targetLayer);
        if (hit.collider == null && currentDirection * -1 != Vector2.down) {
            directions.Add(Vector2.down);
        }

        hit = Physics2D.Raycast(self.position, Vector2.left, .55f, targetLayer);
        if (hit.collider == null && currentDirection * -1 != Vector2.left) {
            directions.Add(Vector2.left);
        }

        hit = Physics2D.Raycast(self.position, Vector2.up, .55f, targetLayer);
        if (hit.collider == null && currentDirection * -1 != Vector2.up) {
            directions.Add(Vector2.up);
        }

        return directions.ToArray();
    }

    public Vector2 GetShortestDirectionIgnore(Vector2 targetPosition, Vector2 currentDirection) {
        Vector2[] availableDirections = GetAvailableDirections(currentDirection, wallLayer);
        float[] costs = new float[availableDirections.Length];
        int i = 0;
        foreach (Vector2 direction in availableDirections) {
            costs[i] = CalculateHCost((Vector2)self.position + direction, targetPosition);
            i++;
        }

        for (int a = 0; a < costs.Length - 1; a++) {
            for (int b = 0; b < costs.Length - a - 1; b++) {
                bool swap = false;
                if (costs[b] > costs[b + 1]) {
                    swap = true;
                }
                else if (costs[b] == costs[b + 1]) {
                    if (availableDirections[b].y < availableDirections[b + 1].y) {
                        swap = true;
                    }
                    else if (availableDirections[b].x > availableDirections[b + 1].x) {
                        swap = true;
                    }
                }

                if (swap) {
                    float temp = costs[b];
                    costs[b] = costs[b + 1];
                    costs[b + 1] = temp;

                    Vector2 tempV = availableDirections[b];
                    availableDirections[b] = availableDirections[b + 1];
                    availableDirections[b + 1] = tempV;
                }
            }
        }

        return availableDirections[0];
    }

    public Vector2 GetShortestDirection(Vector2 targetPosition, Vector2 currentDirection) {
        Vector2[] availableDirections = GetAvailableDirections(currentDirection, allWalls);
        float[] costs = new float[availableDirections.Length];
        int i = 0;
        foreach (Vector2 direction in availableDirections) {
            costs[i] = CalculateHCost((Vector2)self.position + direction, targetPosition);
            i++;
        }

        for (int a = 0; a < costs.Length - 1; a++) {
            for (int b = 0; b < costs.Length - a - 1; b++) {
                bool swap = false;
                if (costs[b] > costs[b + 1]) {
                    swap = true;
                }
                else if (costs[b] == costs[b + 1]) {
                    if (availableDirections[b].y < availableDirections[b + 1].y) {
                        swap = true;
                    }
                    else if (availableDirections[b].x > availableDirections[b + 1].x) {
                        swap = true;
                    }
                }

                if (swap) {
                    float temp = costs[b];
                    costs[b] = costs[b + 1];
                    costs[b + 1] = temp;

                    Vector2 tempV = availableDirections[b];
                    availableDirections[b] = availableDirections[b + 1];
                    availableDirections[b + 1] = tempV;
                }
            }
        }

        return availableDirections[0];
    }

    public Vector2 GetRandomDirection(Vector2 currentDirection, LayerMask targetLayer) {
        List<Vector2> directions = new List<Vector2>();

        RaycastHit2D hit = Physics2D.Raycast(self.position, Vector2.right, .55f, targetLayer);
        if (hit.collider == null && currentDirection * -1 != Vector2.right) {
            directions.Add(Vector2.right);
        }

        hit = Physics2D.Raycast(self.position, Vector2.down, .55f, targetLayer);
        if (hit.collider == null && currentDirection * -1 != Vector2.down) {
            directions.Add(Vector2.down);
        }

        hit = Physics2D.Raycast(self.position, Vector2.left, .55f, targetLayer);
        if (hit.collider == null && currentDirection * -1 != Vector2.left) {
            directions.Add(Vector2.left);
        }

        hit = Physics2D.Raycast(self.position, Vector2.up, .55f, targetLayer);
        if (hit.collider == null && currentDirection * -1 != Vector2.up) {
            directions.Add(Vector2.up);
        }

        if (directions.Count == 1) {
            return directions[0];
        } else {
            int index = Random.Range(0, directions.Count - 1);
            return directions[index];
        }
    }
}
