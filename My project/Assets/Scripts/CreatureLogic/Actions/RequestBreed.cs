using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class RequestBreed : ActionBase
{
    private int action_id = 100;
    private Transform transform;
    private CreatureData data;
    private Stack<Vector3Int> path;
    private Vector3Int next_location;
    private bool running;

    private int breedable = -1;
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
        this.transform = transform;
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
        last_time_accessed = Time.time;
        Dictionary<int, int> creatures_in_range = CreatureManager.instance.GetCreaturesInRange(grid.WorldToCell(transform.position), data.sight_range, CreatureValues.CUR_ENERGY);
        foreach (KeyValuePair<int, int> creature in creatures_in_range)
        {
            if (creature.Value > 30)
            {
                breedable = creature.Key;
                return true;
            }
        }
        
        return false; 
    }

    // Update is called once per frame
    public void Init()
    {
        //get path
        running = CreatureManager.instance.SendRequest(action_id, breedable);
        if (running == false)
        {
            Debug.Log("Request Declined");
            return;
        }

        Debug.Log("Breeding with " + breedable);
        data.energy -= 30;
       
        breedable_locale = CreatureManager.instance.GetCreaturePosition(breedable);
        Debug.Log("Moving to " + breedable_locale);
        path = GenericMovement.MoveTo(grid.WorldToCell(transform.position), breedable_locale);
    }
    public void Run()
    {

        //wait for a nudge back that the request has been accepted
        if (Vector3.Distance(transform.position, grid.GetCellCenterWorld(next_location)) < 0.01f)
        {
            if (grid.WorldToCell(transform.position).Equals(breedable_locale))
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
        transform.position = Vector3.MoveTowards(transform.position, grid.GetCellCenterWorld(next_location), step);
    }

    override
    public string ToString()
    {
        return "Requesting To Breed";
    }
}
