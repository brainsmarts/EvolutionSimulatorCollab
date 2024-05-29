using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class RequestBreed : ActionBase
{
    private int action_id = 100;
    private Transform creature_transform;
    private CreatureData data;
    private Stack<Vector3Int> path;
    private Vector3Int next_location;
    private bool running;

    private Vector3Int breedable_locale;

    public float last_time_accessed = 0;
    public float cooldown = 10f;

    //have time since last time bred
    //have cooldown for when breed can be requested again
    private RangeScanner scanner;

    public RequestBreed()
    {
 
    }
    public void SetTransform(Transform transform)
    {
        this.creature_transform = transform;
    }
    public void SetData(CreatureData data)
    {
        this.data = data;
    }

    public void SetScanner(ref RangeScanner rangeScanner)
    {
        Debug.Log(rangeScanner);
        scanner = rangeScanner;
    }
    public int weight { get; }
    public bool IsRunning()
    {
        
        return running;
    }
    // Start is called before the first frame update
    //public 
    public bool Condition()
    {
        return false;
        //if breeding timer is still going on, then return false
        //only reset the timer if a creature is found and a request is sent
        //multiply it by some enviroment and trait in the future
        if(Time.time - last_time_accessed < cooldown)
        {
            return false;
        }
        if(data.Current_energy < 30){
            return false;
        }
        if ((int)((Time.time - data.TimeBorn) / 60) < 3)
        {
            return false;
        }

        last_time_accessed = Time.time;
        if (scanner == null)
        {
            Debug.Log("Inserting Null Scanner");
        }
        HashSet<BaseCreature> creatures_in_range = scanner.GetCreatures();

        foreach (BaseCreature creature in creatures_in_range)
        {
            if (creature.data.Current_energy > 30)
            {
                data.Target = creature;
                Debug.Log(data.ID + " Has found " + data.Target + "To be very breedable");
                return true;
            }
        }
        
        return false; 
    }

    // Update is called once per frame
    public void Init()
    {
        //get path
        data.Target.SendRequest(action_id, data.ID);
        if (running == false)
        {
            //Debug.Log("Request Declined");
            data.Target = null;
            return;
        }

        Debug.Log("Breeding with " + data.Target);
        data.DecreaseEnergy(30);
       
        //breedable_locale = CreatureManager.instance.GetCreaturePosition(breedable);
        //Debug.Log("Moving to " + breedable_locale);
        //path = GenericMovement.MoveTo(grid.WorldToCell(creature_transform.position), breedable_locale);
    }
    public void Run()
    {
        CreateCreature.instance.BreedNewCreature(data.ID, data.Target.data.ID);
        data.Target = null;
        running = false;
        /*
        //wait for a nudge back that the request has been accepted
        if (Vector3.Distance(creature_transform.position, grid.GetCellCenterWorld(next_location)) < 0.01f)
        {
            if (grid.WorldToCell(creature_transform.position).Equals(breedable_locale))
            {
                //make baby
                running = false;
            }
            else
            {
                if (path.Count <= 0)
                {
                    running = false;
                }
                else
                {
                    next_location = path.Pop();
                }
            }
        }
        var step = 0.5f * Time.deltaTime; // calculate distance to move
        creature_transform.position = Vector3.MoveTowards(creature_transform.position, grid.GetCellCenterWorld(next_location), step);*/

    }

    override
    public string ToString()
    {
        return "Requesting To Breed";
    }
}
