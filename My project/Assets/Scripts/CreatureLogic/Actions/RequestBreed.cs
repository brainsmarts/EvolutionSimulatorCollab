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
    private Grid grid;
    public RequestBreed()
    {
        grid = GameManager.Instance.getGrid();
    }
    public void SetTransform(Transform transform)
    {
        this.creature_transform = transform;
    }
    public void SetData(CreatureData data)
    {
        this.data = data;
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

        last_time_accessed = Time.time;
        Dictionary<int, int> creatures_in_range = CreatureManager.instance.GetCreaturesInRange(grid.WorldToCell(creature_transform.position), data.Sight_range, CreatureValues.CUR_ENERGY);
        foreach (KeyValuePair<int, int> creature in creatures_in_range)
        {
            if (creature.Value > 30)
            {
                data.Target_id = creature.Key;
                Debug.Log(data.ID + " Has found " + data.Target_id + "To be very breedable");
                return true;
            }
        }
        
        return false; 
    }

    // Update is called once per frame
    public void Init()
    {
        //get path
        running = CreatureManager.instance.SendRequest(action_id, data.Target_id);
        if (running == false)
        {
            //Debug.Log("Request Declined");
            data.Target_id = -1;
            return;
        }

        Debug.Log("Breeding with " + data.Target_id);
        data.DecreaseEnergy(30);
       
        //breedable_locale = CreatureManager.instance.GetCreaturePosition(breedable);
        //Debug.Log("Moving to " + breedable_locale);
        //path = GenericMovement.MoveTo(grid.WorldToCell(creature_transform.position), breedable_locale);
    }
    public void Run()
    {
        CreateCreature.instance.BreedNewCreature(data.ID, data.Target_id);
        data.Target_id = -1;
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
