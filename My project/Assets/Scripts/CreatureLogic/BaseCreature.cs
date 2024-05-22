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
    private float idle_length;
    private float idle_timer;

    [SerializeField]
    private Animator animator;
    private Grid grid;
    private bool idle = true;


    // Start is called before the first frame update
    void Start()
    {
        grid = GameManager.Instance.getGrid();

        data.request_id = -1;
        data.target_id = -1;
        data.requester_id = -1;

        //setting all possible actions
        actions = new List<ActionBase>();
        FindFood find_food = new FindFood();
        find_food.SetData(data);
        find_food.SetTransform(transform);

        Wander wander = new Wander();
        wander.SetData(data);
        wander.SetTransform(transform);

        ResponseAccepter response = new ResponseAccepter();
        response.SetData(data);
        response.SetTransform(transform);

        RequestBreed rb = new RequestBreed();
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
    }

    private void ResetTimer()
    {
        idle = true;
        idle_timer = idle_length;
    }

    private void Die()
    {

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

        data.request_id = request_id;
        data.requester_id = creature_id;
        return true;
    }

    public string GetCurrentAction()
    {
        if (current_action == null)
            return "idle";

        return current_action.ToString();
    }
}
