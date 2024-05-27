using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneTemplate;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.Tilemaps;

public class CreateCreature : MonoBehaviour
{

    public static CreateCreature instance;
    [SerializeField] private Tilemap world_map;
    private BoundsInt map_border;

    void Start(){
        instance = this;
        id = 2;
        map_border = world_map.cellBounds;
        for(int i = 0; i < 10; i++)
        {
            SpawnCreature();
        }
    }
    public Sprite sprite;
    private int id;

    public GameObject creature_holder;

    //at x position
    public void SpawnCreature(){
        id++;
        int random_x = Random.Range(map_border.xMin, map_border.xMax);
        int random_y = Random.Range(map_border.yMin, map_border.yMax);
        GameObject creature =new();
        CreatureData data = new(id, 100, Random.Range(1,10), 8);
        data.SetActions(CreateActions(creature.transform, data));

        creature.transform.position = GameManager.Instance.getGrid().CellToWorld(new
            Vector3Int(Random.Range(map_border.xMin, map_border.xMax), Random.Range(map_border.yMin, map_border.yMax)));

        creature.transform.parent = creature_holder.transform;
        creature.AddComponent<BaseCreature>();

        //creature.transform.position = 

        BaseCreature baseCreature = creature.GetComponent<BaseCreature>();
        
        baseCreature.SetData(data);

        creature.AddComponent<SpriteRenderer>();
        SpriteRenderer spriteR = creature.GetComponent<SpriteRenderer>();
        spriteR.sprite = sprite;
        spriteR.sortingOrder = 5;
        //Instantiate(creature);
        Debug.Log("Creature Created");

        CreatureManager.instance.AddCreature(baseCreature);
    }

    public void BreedNewCreature(int parent1, int parent2){
        id++;
        //get data from both parents
        CreatureData data1 = CreatureManager.instance.GetData(parent1);
        CreatureData data2 = CreatureManager.instance.GetData(parent2);
        //create new Data using the data from parent 1 and parent 2
        
        //create new game object 
        GameObject new_creature = new();    
        
        CreatureData data3 = CreateData(data1, data2, new_creature.transform);
        new_creature.AddComponent<BaseCreature>();
        //set the new data to the creature and add base creature component
        //new_creature.GetComponent<BaseCreature>().SetData(new());
        BaseCreature creature_base = new_creature.GetComponent<BaseCreature>();
        creature_base.SetData(data3);
        CreatureManager.instance.AddCreature(creature_base);

        new_creature.AddComponent<SpriteRenderer>();
        SpriteRenderer spriteR = new_creature.GetComponent<SpriteRenderer>();
        spriteR.sprite = sprite;
        spriteR.sortingOrder = 5;
    }

    private CreatureData CreateData(CreatureData parent1, CreatureData parent2, Transform creature_transform){
        CreatureData data;
        int min;
        int max;

        min = parent1.Energy < parent2.Energy ? parent1.Energy : parent2.Energy;
        max = parent1.Energy > parent2.Energy ? parent1.Energy : parent2.Energy;
        int energy = Random.Range(min-1, max + 1);

        min = parent1.Sight_range < parent2.Sight_range ? parent1.Sight_range : parent2.Sight_range;
        max = parent1.Sight_range > parent2.Sight_range ? parent1.Sight_range : parent2.Sight_range;
        int sight_range = Random.Range(min -1, max +1);

        min = parent1.Speed < parent2.Speed ? parent1.Speed : parent2.Speed;
        max = parent1.Speed  > parent2.Speed ? parent1.Speed : parent2.Speed;
        int speed = Random.Range(min -1, max +1);
        data = new(id, energy, speed, sight_range);
        data.SetActions(CreateActions(creature_transform, data));
        return data;
    }

    private List<ActionBase> CreateActions(Transform creature_transform,CreatureData data){
        List<ActionBase> actions = new();
        FindFood find_food = new();
        find_food.SetData(data);
        find_food.SetTransform(creature_transform);

        Wander wander = new();
        wander.SetData(data);
        wander.SetTransform(creature_transform);

        ResponseAccepter response = new();
        response.SetData(data);
        response.SetTransform(creature_transform);

        RequestBreed rb = new();
        rb.SetData(data);
        rb.SetTransform(creature_transform);

        actions.Add(response);
        actions.Add(find_food);
        actions.Add(rb);
        actions.Add(wander);
        return actions;
    }
}
