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
    [SerializeField] GameObject creature_prefab;
    [SerializeField] int NumOfStartingCreatures;

    void Start(){
        instance = this;
        id = 2;
        map_border = world_map.cellBounds;
        for(int i = 0; i < NumOfStartingCreatures; i++)
        {
            SpawnCreature();
        }
    }
    private int id;

    public GameObject creature_holder;

    //at x position
    public void SpawnCreature(){
        id++;
        int random_x = Random.Range(map_border.xMin, map_border.xMax);
        int random_y = Random.Range(map_border.yMin, map_border.yMax);
        GameObject creature =Instantiate(creature_prefab, creature_holder.transform);
        

        CreatureData data = new(id, 100, Random.Range(30,40), 8, new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)));
        BaseCreature baseCreature = creature.GetComponent<BaseCreature>();
        data.SetActions(CreateActions(creature.GetComponent<Rigidbody2D>(), data, creature.GetComponentInChildren<RangeScanner>()));

        creature.transform.position = GameManager.Instance.getGrid().CellToWorld(new
            Vector3Int(Random.Range(map_border.xMin, map_border.xMax), Random.Range(map_border.yMin, map_border.yMax)));
        //creature.transform.position = 

        
        
        baseCreature.SetData(data);
        SpriteRenderer spriteR = creature.GetComponent<SpriteRenderer>();
        spriteR.color = data.Color;
        //Instantiate(creature);
        //Debug.Log("Creature Created");

        CreatureManager.instance.AddCreature(baseCreature);
    }

    public void BreedNewCreature(int parent1, int parent2){
        id++;
        //get data from both parents
        CreatureData data1 = CreatureManager.instance.GetData(parent1);
        CreatureData data2 = CreatureManager.instance.GetData(parent2);
        //create new Data using the data from parent 1 and parent 2
        
        //create new game object 
        GameObject new_creature = Instantiate(creature_prefab, creature_holder.transform);
        BaseCreature creature_base = new_creature.GetComponent<BaseCreature>();

        if (new_creature.GetComponentInChildren<RangeScanner>() == null) {
            Debug.Log("New Creatures Do Not Have Range Scanner");
        }
        CreatureData data3 = CreateData(data1, data2, new_creature.GetComponent<Rigidbody2D>(), new_creature.GetComponentInChildren<RangeScanner>());

        //set the new data to the creature and add base creature component
        //new_creature.GetComponent<BaseCreature>().SetData(new());
        //BaseCreature creature_base = new_creature.GetComponent<BaseCreature>();
        creature_base.SetData(data3);
        CreatureManager.instance.AddCreature(creature_base);

        
        SpriteRenderer spriteR = new_creature.GetComponent<SpriteRenderer>();
        spriteR.color = data3.Color;

    }

    private CreatureData CreateData(CreatureData parent1, CreatureData parent2, Rigidbody2D creature_rb, RangeScanner scanner){
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

        Color color = Color.Lerp(parent1.Color, parent2.Color, 1);
        data = new(id, energy, speed, sight_range, color);
        data.SetActions(CreateActions(creature_rb, data, scanner));
        return data;
    }

    private List<ActionBase> CreateActions(Rigidbody2D creature_rb, CreatureData data, RangeScanner scanner){
        List<ActionBase> actions = new();
        FindFood find_food = new();
        find_food.SetData(data);
        find_food.SetRigidBody(creature_rb);
        find_food.SetScanner(ref scanner);

        Wander wander = new();
        wander.SetData(data);
        wander.SetRigidBody(creature_rb);
        wander.SetScanner(ref scanner);

        ResponseAccepter response = new();
        response.SetData(data);
        response.SetRigidBody(creature_rb);
        response.SetScanner(ref scanner);   

        RequestBreed rb = new();
        rb.SetData(data);
        rb.SetRigidBody(creature_rb);
        rb.SetScanner(ref scanner);

        actions.Add(response);
        actions.Add(find_food);
        actions.Add(rb);
        actions.Add(wander);
        return actions;
    }
}
