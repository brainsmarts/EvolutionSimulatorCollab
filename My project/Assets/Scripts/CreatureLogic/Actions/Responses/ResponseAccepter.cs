using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResponseAccepter : ActionBase
{
    // Start is called before the first frame update
    private Transform transform;
    private CreatureData data;
    private bool running = false;
    Response response;
    public ResponseAccepter()
    {

    }
    public void SetTransform(Transform transform) => this.transform = transform;
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
        //requests are empty 
        if (data.request_id < 0)
        {
            return false;
        }
        Debug.Log("Response Accepted");
        
        //return response.GetCondition();
        return true;
    }

    // Update is called once per frame
    public void Init()
    {
        //get path
        Debug.Log(data.request_id);
        response = Response.GetResponse(data.request_id, transform, data);
        response.Init();
        running = true;
    }
    public void Run()
    {
        running = response.RunResponse();
        if(running == false)
        {
            data.requester_id = -1;
            data.request_id = -1;
        }
    }

    override
    public string ToString()
    {
        return "Accepting Response";
    }
}
