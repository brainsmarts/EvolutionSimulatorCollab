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
    [SerializeField]
    private TileBase food_tile;
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
        
        food_tiles.Add(new Vector3Int(random_x, random_y), 10);
        food_map.SetTile(new Vector3Int(random_x, random_y), food_tile);
        
        

    }

    public Vector3Int FoodInRange(Vector3Int player_position, int range)
    {
        Vector3Int food_location = player_position;
        foreach (KeyValuePair<Vector3Int, int> food in food_tiles)
        {
            //Debug.Log(food.Key);
            float distance = Vector3Int.Distance(player_position, food.Key);
            if (distance <= range) {
                if (distance < Vector3Int.Distance(player_position, food_location) || food_location == player_position)
                    food_location = food.Key;
            }
        }
        //Debug.Log("Food Not Found");
        return food_location;
    }

    public int EatFood(Vector3Int location)
    {
        KeyValuePair<Vector3Int, int> food_to_eat = default;
        foreach (KeyValuePair<Vector3Int, int> food in food_tiles)
        {
            if(Vector3Int.Distance(location, food.Key) == 0)
            {
                food_to_eat = food;
                
            }
        }

        if (!food_to_eat.Equals(default(KeyValuePair<Vector3Int, int>)))
        {
            food_tiles.Remove(food_to_eat.Key);
            food_map.SetTile(food_to_eat.Key, null);
            return food_to_eat.Value;
        }

        return 0;
    }
}
