using System;
using System.Collections.Generic;
using UnityEngine;

public class BaseCreature : MonoBehaviour
{

    [SerializeField]
    public CreatureData data;
    [SerializeField]
    private RangeScanner scanner;

    [SerializeField]
    private Rigidbody2D rb;
    
    private Vector3Int next_location;

    private List<ActionBase> actions;
    public ActionBase current_action { get; private set; }

    [SerializeField]
    private float idle_length = 4;
    private float idle_timer = 0;

    [SerializeField]
    private float metobolism_rate = 10;
    private float metobolism_timer = 0;

    //[SerializeField]
    //private Animator animator;
    private Grid grid;
    private bool idle = true;
   


    // Start is called before the first frame update
    void Start()
    {
        //set a random location to head to
        grid = GameManager.Instance.getGrid();

        Vector3Int current_position = grid.WorldToCell(transform.position);
        data.Target_Location = grid.CellToWorld(new Vector3Int(current_position.x + UnityEngine.Random.Range(5,10), 
            current_position.y + UnityEngine.Random.Range(5,10)));

        data.SetNewTargetLocation(current_position);
        next_location = data.path.Pop();
        scanner.SetRange(data.Sight_range);
        data.Target = null;
        data.Request_id = -1;

        scanner.Enable();

    }


    // Update is called once per frame
    void FixedUpdate()
    {
        DoAction();
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
        if(current_action != null)
        {
            if (current_action.IsRunning() == false)
            {
                current_action = null;
            }
        }
        

        if(Vector3.Distance(transform.position, grid.CellToWorld(next_location)) < 0.05f)
        {
            //set new location if final location is reached
            //else pop
            if(Vector3.Distance(transform.position, data.Target_Location) < 0.05f)
            {
                Vector3Int current_position = grid.WorldToCell(transform.position);
                data.Target_Location = grid.CellToWorld(new Vector3Int(current_position.x + UnityEngine.Random.Range(5, 10),
                    current_position.y + UnityEngine.Random.Range(5, 10)));
            }
            else
            {
                next_location = data.path.Pop();
            }
        }

        foreach (ActionBase action in actions)
        {
            if (action.Condition())
            {
                action.Init();
                current_action = action;
                break;
            }
        }
        if (current_action != null)
        {
            if (current_action.IsRunning())
            {
                current_action.Run();
            }
        }

        rb.velocity = new Vector2(data.Target_Location.x - transform.position.x, data.Target_Location.y - transform.position.y).normalized * data.Speed * Time.deltaTime;
    }

    private void ResetTimer()
    {
        idle = true;
        idle_timer = idle_length;
    }

    private void CheckDeath()
    {
        if (data.Current_energy <= 0)
        {
            CreatureManager.instance.RemoveCreature(this);
            Destroy(gameObject);
        } 
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

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
    }

    public string GetCurrentAction()
    {
        if (current_action == null)
            return "idle";

        return current_action.ToString();
    }

    public void SetData(CreatureData data){
        this.data = data;
        this.actions = data.Actions;
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
