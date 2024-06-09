using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField]
    private Grid grid;
    [SerializeField]
    private Tilemap world_map;
    [SerializeField]
    private float time_scale = 1;

    [SerializeField]
    private Tilemap terrain_map;

    private BoundsInt map_border;
    private int minx, maxx, miny, maxy; 
    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        map_border = world_map.cellBounds;
        minx = map_border.xMin; maxx = map_border.xMax;
        miny = map_border.yMin; maxy = map_border.yMax;
        Time.timeScale = time_scale;    
    }

    // Update is called once per frame

    public Grid getGrid()
    {
        return grid;
    }

    public bool OutOfBounds(Vector3Int position)
    {
        Debug.Log("Out of bounds Logging + /nDesiredPosiiton: "+ position.x + " " + position.y + 
            "\nMinimumBoundry" + minx + " " + miny +
            "\nMaximumBoundry" + maxx + " " + maxy);
        return position.x >= maxx || position.x <= minx || position.y >= maxy || position.y <= miny;
    }

    public bool IsNotRock(Vector3Int position)
    {
        //Debug.Log("Position " + position + " Is not rock " + terrain_map.GetTile(position) == null);
        return terrain_map.GetTile(position) == null;
    }
}

