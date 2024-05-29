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
    private RangeScanner scanner;

    public ResponseAccepter()
    {

    }
    public void SetTransform(Transform transform) => this.transform = transform;
    public void SetData(CreatureData data)
    {
        this.data = data;
    }

    public void SetScanner(ref RangeScanner rangeScanner)
    {
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
        //requests are empty 
        if (data.Target == null)
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
        //Debug.Log(data.Request_id);
        response = Response.GetResponse(data.Request_id, transform, data);
        if(response == null)
            Debug.Log("Evil");
        response.Init();
        running = true;
    }
    public void Run()
    {
        running = response.RunResponse();   
        if(running == false)
        {
            data.Request_id = -1;
            data.Target = null;
        }
    }

    override
    public string ToString()
    {
        return "Accepting Response";
    }
}
