using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCreature : MonoBehaviour
{
    
    [SerializeField]
    private CreatureData data;

    private List<ActionBase> actions;
    private ActionBase current_action = null;

    [SerializeField]
    private float idle_length = 4;
    private float idle_timer;

    //[SerializeField]
    //private Animator animator;
    private Grid grid;
    private bool idle = true;


    // Start is called before the first frame update
    void Start()
    {
        grid = GameManager.Instance.getGrid();

        data.Target_id = -1;
        data.Request_id = -1;

        //setting all possible actions
        actions = new List<ActionBase>();
        FindFood find_food = new();
        find_food.SetData(data);
        find_food.SetTransform(transform);

        Wander wander = new();
        wander.SetData(data);
        wander.SetTransform(transform);

        ResponseAccepter response = new();
        response.SetData(data);
        response.SetTransform(transform);

        RequestBreed rb = new();
        rb.SetData(data);
        rb.SetTransform(transform);

        actions.Add(response);
        actions.Add(rb);
        actions.Add(find_food);
        actions.Add(wander);
    }


    // Update is called once per frame
    void FixedUpdate()
    {

        if (idle)
        {
            if (idle_timer > 0)
            {
                idle_timer -= Time.deltaTime;
            }
            else
            {
                idle = false;
            }
        }
        else
        {
            if(current_action == null)
            {
                foreach (ActionBase action in actions)
                {
                    if (action.Condition()) 
                    {
                        action.Init();
                        current_action = action;
                        break;
                    }

                }
            }
            if (current_action.IsRunning())
            {
                //Debug.Log(current_action.ToString());
                current_action.Run();
            }
            else
            {
                current_action = null;
                ResetTimer();
            }
            
        }
        if(data.Current_energy <= 0){
            Die();
        }
    }

    private void ResetTimer()
    {
        idle = true;
        idle_timer = idle_length;
    }

    private void Die()
    {
        CreatureManager.instance.RemoveCreature(this);
        Destroy(gameObject);
    }

    public Vector3 GetGridPosition()
    {
        return transform.position;
    }

    public ref CreatureData GetStats()
    {
        return ref data;
    }

    public int GetID()
    {
        return data.ID;
    }

    internal bool SendRequest(int request_id, int creature_id)
    {
        bool response = Response.Reply(request_id, data);
        if(response == false)
            return false;

        if (current_action != null)
            return false;

        if(data.Request_id != -1)
            return false;
        

        data.Target_id = creature_id;
        data.Request_id = request_id;
        return true;
    }

    public string GetCurrentAction()
    {
        if (current_action == null)
            return "idle";

        return current_action.ToString();
    }

    public void SetData(CreatureData data){
        this.data = data;
    }

    public CreatureData GetData(){
        return data;
    }
}
