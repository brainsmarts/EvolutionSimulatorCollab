using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResponseAccepter : ActionBase
{
    // Start is called before the first frame update
    private Rigidbody2D rb;
    private CreatureData data;
    private bool running = false;
    Response response;
    private RangeScanner scanner;

    public ResponseAccepter()
    {

    }
    public void SetRigidBody(Rigidbody2D rb) => this.rb = rb;
    public void SetData(CreatureData data)
    {
        this.data = data;
    }

    public void SetScanner(RangeScanner rangeScanner)
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
        //Debug.Log("Response Accepted");
        
        //return response.GetCondition();
        return true;
    }

    // Update is called once per frame
    public void Init()
    {
        //get path
        //Debug.Log(data.Request_id);
        //response = Response.GetResponse(data.Request_id, rb, data, scanner);
        response.Init();
        running = true;
    }
    public void Run()
    {
        running = response.RunResponse();   
        if(running == false)
        {
            //data.Request_id = -1;
            data.Target = null;
        }
    }

    override
    public string ToString()
    {
        return "Accepting Response";
    }
}
