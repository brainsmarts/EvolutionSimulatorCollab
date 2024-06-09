using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GenerateTerrain : MonoBehaviour
{
    Grid grid;
    [SerializeField]
    Tilemap map;
    Tilemap terrain;
    BoundsInt bounds;
    // Start is called before the first frame update
    void Start()
    {
        grid = GameManager.Instance.getGrid();
        bounds = map.cellBounds;
        for(int i = bounds.xMin; i <= bounds.xMax; i++){
            for(int j = bounds.yMin; i <= bounds.yMax; j++){
                float perlin = Mathf.PerlinNoise(i,j);
                if(perlin <= 0.01f){
                    //create BoulderTile
                    //add boulder tile
                }else if(perlin <= 0.07){
                    //create WaterTile?
                    //add boulder tile
                }
            }
        }

    }   
}
