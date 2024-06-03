using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using UnityEngine;

public class Wander : ActionBase
{

    public int weight { get; }
    public Rigidbody2D rb;
    public CreatureData data;
    private Grid grid;
    private Vector2 new_position;
    public bool running;
    private Time time_since_wandered;
    private int direction_bias = -1;
    private int steps_counter = 0;
    private int steps;
    int[,] directions = new int[8, 2] {
            {1,0},
            {-1,0},
            {0,1},
            {0,-1},
            {1,1},
            {1,-1},
            {-1,1}, 
            {-1,-1},
        };


    private Stack<Vector3Int> path;

    public Wander()
    {
        weight = 0;
        grid = GameManager.Instance.getGrid();
    }

    public void SetRigidBody(Rigidbody2D rb)
    {
        this.rb = rb;
    }

    public void SetData(CreatureData data)
    {
        this.data = data;
    }

    public void SetScanner(ref RangeScanner scanner)
    {

    }

    public bool IsRunning()
    {
        return running; 
    }

    public bool Condition()
    {
        return true;
    }

    public void Init()
    {
        //Debug.Log("Wander Start");
        steps = 1;
        new_position = GetNextPath();
        //path = GenericMovement.MoveTo();
        data.DecreaseEnergy(1);
        running = true;
    }

    public void Run()
    {
        /*
        if(Vector2.Distance(new_position, rb.position) < .01f)
        {
            //Debug.Log(new_position + " " + rb.position);
            if (steps_counter >= steps)
            {
                //Debug.Log("Wandering Finished");
                rb.velocity *= 0;
                //Debug.Log(rb.velocity.ToString());
                running = false;
                return;
            }
            else
            {
                steps_counter++;
                data.DecreaseEnergy(1);
                new_position = GetNextPath();
            }
        }
       

        rb.velocity = new Vector2(new_position.x - rb.position.x, new_position.y - rb.position.y).normalized * data.Speed * Time.deltaTime;
        //Debug.DrawLine(rb.position, rb.velocity.normalized + rb.position, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(rb.position, rb.velocity.normalized, .05f);
        if(hit.collider != null)
        {
            float left = 0;
            if(rb.velocity.x > rb.velocity.y)
            {
                left = rb.velocity.x > 0 ? .1f : -.1f;
                rb.AddForce(new(left, 0));
            }
            else
            {
                left = rb.velocity.y > 0 ? .1f : -.1f;
                rb.AddForce(new(0, left));
            }
        }
        //Debug.Log(data.Speed);
        */

    }

    private Vector3 GetNextPath()
    {
        List<Vector3Int> neighboors = new List<Vector3Int>();
        Vector3Int path;
        int current_x = grid.WorldToCell(rb.position).x;
        int current_y = grid.WorldToCell(rb.position).y;

        for (int i = 0; i < directions.GetLength(0); i++)
        {
            Vector3Int neighboor = new Vector3Int(current_x + directions[i, 0], current_y + directions[i, 1]);
            if (!GameManager.Instance.OutOfBounds(neighboor) && GameManager.Instance.IsNotRock(neighboor))
            {
                neighboors.Add(new Vector3Int(current_x + directions[i, 0], current_y + directions[i, 1]));
            }
            else
            {
                direction_bias = -1;
            }
            
        }

        if (Random.Range(0,1) < .35f && direction_bias != -1)
        {
            //Debug.Log(direction_bias + " " + neighboors.Count);
            path = neighboors[direction_bias];  
        }
        else
        {
            int random = Random.Range(0, neighboors.Count);
            direction_bias = random;
            //Debug.Log(random);
            path = neighboors[random];
        }
        data.DecreaseEnergy(1);
        return grid.GetCellCenterWorld(path);
    }

    override
    public string ToString()
    {
        return "Wandring";
    }
}
