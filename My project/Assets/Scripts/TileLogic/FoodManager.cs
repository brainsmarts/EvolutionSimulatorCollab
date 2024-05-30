using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FoodManager : MonoBehaviour
{
    [SerializeField]
    private int StartingFoodAmount;
    [SerializeField]
    private Tilemap food_map;
    [SerializeField]
    private Tilemap world_map;
    [SerializeField] Transform food_holder;
    [SerializeField] private GameObject food;
    [SerializeField]
    private float food_spawn_rate = 5f;
    private float food_spawn_timer;
    private BoundsInt map_border;

    private Dictionary<Vector3Int, int> food_tiles = new();
    public static FoodManager Instance { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        map_border = world_map.cellBounds;
        food_spawn_timer = food_spawn_rate;
        for(int i = 0; i < StartingFoodAmount; i++)
        {
            RandomAddFood();    
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(food_spawn_timer < 0)
        {
            RandomAddFood();
            food_spawn_timer = food_spawn_rate;
        }
        else
        {
            food_spawn_timer -= Time.deltaTime;
        }
    }

    private void RandomAddFood()
    {
        int random_x = Random.Range(map_border.xMin, map_border.xMax);
        int random_y = Random.Range(map_border.yMin, map_border.yMax);
        int max_tries = 2;


        while (food_tiles.ContainsKey(new Vector3Int(random_x, random_y)) && max_tries > 0)
        {
            random_x = Random.Range(map_border.xMin, map_border.xMax);
            random_y = Random.Range(map_border.yMin, map_border.yMax);
            max_tries--;
        }

        //food_tiles.Add(new Vector3Int(random_x, random_y), 15);
        GameObject something = Instantiate(food);
        something.transform.position = GameManager.Instance.getGrid().GetCellCenterWorld(new Vector3Int(random_x, random_y));
        something.transform.parent = food_holder;

        //food_map.SetTile(new Vector3Int(random_x, random_y), food_tile);
    }
}
