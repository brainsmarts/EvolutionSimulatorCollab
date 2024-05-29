using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class Wander : ActionBase
{

    public int weight { get; }
    public Transform transform;
    public CreatureData data;
    private Grid grid;
    private Vector3Int new_position;
    public bool running;
    private Time time_since_wandered;
    private int direction_bias = -1;
    private int steps_counter = 0;
    private int steps;
    int[,] directions = new int[4, 2] {
            {1,0},
            {-1,0},
            {0,1},
            {0,-1},
        };

    public Wander()
    {
        weight = 0;
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
        steps = Random.Range(1, 6);
        new_position = GetNextPath();
        data.DecreaseEnergy(1);
        running = true;
    }

    public void Run()
    {
        if (Vector3.Distance(transform.position, grid.GetCellCenterWorld(new_position)) < 0.05f)
        {
            if (steps_counter >= steps)
            {
                running = false;
            }
            else
            {
                steps_counter++;
                new_position = GetNextPath();
            }

        }
        //Debug.Log(data.Speed);
        var step = 0.05f * data.Speed * Time.deltaTime; // calculate distance to move
        transform.position = Vector3.MoveTowards(transform.position, grid.GetCellCenterWorld(new_position), step);
    }

    private Vector3Int GetNextPath()
    {
        List<Vector3Int> neighboors = new List<Vector3Int>();
        Vector3Int path;
        int current_x = grid.WorldToCell(transform.position).x;
        int current_y = grid.WorldToCell(transform.position).y;

        for (int i = 0; i < directions.GetLength(0); i++)
        {
            Vector3Int neighboor = new Vector3Int(current_x + directions[i, 0], current_y + directions[i, 1]);
            if (!GameManager.Instance.OutOfBounds(neighboor))
            {
                neighboors.Add(new Vector3Int(current_x + directions[i, 0], current_y + directions[i, 1]));
            }
            else
            {
                direction_bias = -1;
            }
            
        }

        if (Random.Range(0,1) < .4f && direction_bias != -1)
        {
            //Debug.Log(direction_bias + " " + neighboors.Count);
            path = neighboors[direction_bias];  
        }
        else
        {
            int random = Random.Range(0, neighboors.Count);
            direction_bias = random;        
            path = neighboors[random];
        }
        data.DecreaseEnergy(1);
        return path;
    }

    override
    public string ToString()
    {
        return "Wandring";
    }
}
