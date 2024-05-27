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
    private BoundsInt map_border;
    private int minx, maxx, miny, maxy; 
    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        map_border = world_map.cellBounds;
        minx = map_border.xMin; maxx = map_border.xMax;
        miny = map_border.yMin; maxy = map_border.yMax;
    }

    // Update is called once per frame

    public Grid getGrid()
    {
        return grid;
    }

    public bool OutOfBounds(Vector3Int position)
    {
        /*
        Debug.Log(position.x >= maxx || position.x <= minx || position.y >= maxy || position.y <= miny);
        Debug.Log(position.x + " :" + maxx + " " + minx);
        Debug.Log(position.y + " :" + maxy + " " + miny);*/
        return position.x >= maxx || position.x <= minx || position.y >= maxy || position.y <= miny;
    }

    
}

