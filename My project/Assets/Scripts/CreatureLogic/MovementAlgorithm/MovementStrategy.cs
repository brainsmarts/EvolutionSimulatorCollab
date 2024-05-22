using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface MovementStrategy
{
    public Vector3Int NewPosition(Vector3Int position, Grid grid);
}
