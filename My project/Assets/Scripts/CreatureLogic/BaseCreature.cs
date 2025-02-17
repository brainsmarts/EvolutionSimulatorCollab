using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class BaseCreature : MonoBehaviour
{

    [SerializeField]
    public CreatureData data;
    [SerializeField]
    private RangeScanner scanner;

    [SerializeField]
    private Rigidbody2D rb;
    [SerializeField]
    private Collider2D collider;
    
    //private List<ActionBase> actions;
    public ActionBase current_action { get; private set; }


    [SerializeField]
    private float metobolism_rate = 10;
    private float metobolism_timer = 0;




    // Start is called before the first frame update
    void Start()
    {
        scanner.SetRange(data.Sight_range);
        data.Target = null;
        scanner.Enable();
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        DoAction();
        MoveToTargetLocation();
        CheckMetobolism();
        CheckDeath();
    }

    private void CheckMetobolism()
    {
        if (metobolism_timer > metobolism_rate)
        {
            data.DecreaseEnergy(1);
            metobolism_timer = 0;
        }
        else
        {
            metobolism_timer += Time.deltaTime; 
        }
    }
    private void DoAction()
    {
        //replace null with idle
        if(current_action == null){
            foreach (ActionBase action in data.Actions)
                {
                    if (action.Condition())
                    {
                        action.Init();
                        current_action = action;
                        break;
                    }
                }
        }else{
            if(current_action.IsRunning()){
                current_action.Run();
            }else{
                current_action = null;
            }
        }
     
    }

    private void MoveToTargetLocation(){
        //Debug.Log(grid.WorldToCell(transform.position) + " < > " + grid.WorldToCell(data.Target_Location));
        if (data.path.Count == 0)
        {
            //replace set path with a action
            Debug.Log("New Path Set");
            data.SetRandomPath();
            /*
            Stack<Vector3Int> temp = new Stack<Vector3Int>(data.path);
            foreach (Vector3Int pathway in temp)
            {
                Debug.Log("Random Path: " + grid.GetCellCenterWorld(pathway));
            }*/
            return;
        }
        
        //Condition comparing if distance is less than 0.01 is sketchy
        if (Vector3.Distance(transform.position,data.Target_Location) < 0.01){
            Debug.Log("Target Reached");
            Debug.Log("Next In Path");
            data.NextInPath();
        }

        rb.velocity = new Vector2(data.Target_Location.x - transform.position.x, data.Target_Location.y - transform.position.y).normalized * data.Speed * Time.deltaTime;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, rb.velocity, .2f);
        Debug.DrawRay(transform.position, rb.velocity.normalized * 0.1f, Color.red);
      
        if (hit.collider != null && !collider.gameObject.Equals(gameObject))
        {
            Debug.Log("Stuck");
            float newx, newy;
            newx = -rb.velocity.y;
            newy = rb.velocity.x;
            rb.AddForce(new Vector2(newx, newy).normalized, ForceMode2D.Impulse);
        }
    }

    private void CheckDeath()
    {
        if (data.Current_energy <= 0)
        {
            Debug.Log("I Died");
            //CreatureManager.instance.RemoveCreature(this);
            Destroy(gameObject);
        } 
    }

    /*
    internal bool SendRequest(int request_id, int creature_id)
    {
        bool response = Response.Reply(request_id, data);
        if(response == false)
            return false;   

        if(data.Request_id != -1)
            return false;
        

        data.Target = scanner.Find(creature_id);
        if(data.Target == null)
        {
            return false;
        }
        data.Request_id = request_id;
        return true;
    }*/

    public void SetData(CreatureData data){
        this.data = data;
    }

    public int GetAge()
    {
        return (int)((Time.time - data.TimeBorn) / 60); 
    }
    
    public Transform GetTransform() { return transform; }

    private void OnMouseDown()
    {
        DebugManager.Instance.Display(this);
    }
}
