using System;
using System.Collections.Generic;
using System.IO;
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

        //data.SetNewTargetLocation(current_position);
        //next_location = data.path.Pop();
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
        foreach (ActionBase action in actions)
        {
            if (action.Condition())
            {
                action.Init();
                action.Run();
                break;
            }
        }

        //move this to Data since this check should only happen once
        if (data.path == null)
        {
            Vector3Int current_position = grid.WorldToCell(transform.position);
            Vector3Int new_target_position = new Vector3Int(current_position.x + UnityEngine.Random.Range(5, 10),
                    current_position.y + UnityEngine.Random.Range(5, 10));
            
            if (GameManager.Instance.OutOfBounds(new_target_position))
            {
                Debug.Log(new_target_position);
                Debug.Log("Out of bounds");
            }

            data.SetNewTargetLocation(grid.CellToWorld(new_target_position));
            next_location = data.path.Pop();
            Debug.Log(next_location);
            Debug.Log(grid.WorldToCell(data.Target_Location));
            return;
        }

        if(grid.WorldToCell(transform.position).Equals(grid.WorldToCell(next_location)))
        {
            Debug.Log("Next Location Reached");
            if(Vector3.Distance(transform.position, data.Target_Location) < 0.09f)
            {
                Debug.Log("Final Location Reached");
                Vector3Int current_position = grid.WorldToCell(transform.position);
                data.Target_Location = grid.CellToWorld(new Vector3Int(current_position.x + UnityEngine.Random.Range(5, 10),
                    current_position.y + UnityEngine.Random.Range(5, 10)));
            }
            else
            {
                next_location = data.path.Pop();
            }
        }

        rb.velocity = new Vector2(next_location.x - transform.position.x, next_location.y - transform.position.y).normalized * data.Speed / 2 * Time.deltaTime;
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
            Debug.Log("I Died");
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
