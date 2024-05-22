using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMovement : MovementStrategy
{
    public Vector3Int NewPosition(Vector3Int position, Grid grid)
    {
        Vector3Int new_position = position;
        float random = Random.Range(0f, 4f);
        int x = random < 1 ? 1 : random < 2 ? -1 : 0;
        int y = x == 0 ? random < 3 ? 1 : random < 4 ? -1: 0 : 0;
        grid.GetCellCenterWorld(position);
        new_position.x += x;
        new_position.y += y;    
        return new_position;
    }
}
