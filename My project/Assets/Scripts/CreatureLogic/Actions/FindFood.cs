using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FindFood : ActionBase
{
    public int weight { get; }
    public Transform transform;
    public CreatureData data;
    private RangeScanner scanner;
    private Grid grid;
    private FoodScript food;
    private Vector3Int next_location;
    private Stack<Vector3Int> path;
    public bool running;

    public FindFood()
    {
        weight = 1;
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

    public void SetScanner(ref RangeScanner rangeScanner)
    {
        
        scanner = rangeScanner;
    }

    public bool IsRunning()
    {
        return running;
    }

    public bool Condition()
    {
        //Debug.Log("Is There food?");
        if (data.IsFull())
        {
            return false;
        }

        food = scanner.GetNearestFood();  
        return food != null;
    }


    public void Init()
    {
        //List<Vector3Int> neighboors = GenericMovement.GetNeighboors(grid.WorldToCell(transform.position));
        //Debug.Log("Food Init");
        if (food == null)
        {
            running = false;
            return;
        }
            
        
        path = GenericMovement.MoveTo(grid.WorldToCell(transform.position), grid.WorldToCell(food.GetPosition())) ;
        next_location = path.Pop();
        running = true;
    }

    public void Run()
    {
        if (Vector3.Distance(transform.position, grid.GetCellCenterWorld(next_location)) < 0.01f)
        {
            if (food == null) { 
                running = false;
                    return;
            }
            if (food.InRange(transform))
            {
                int energy_gained = food.EatFood();
                data.IncreaseEnergy(energy_gained);
                running = false;
            }
            else
            {
                next_location = path.Pop();
                data.DecreaseEnergy(1);
            }
        }
        var step = 0.05f * data.Speed * Time.deltaTime; // calculate distance to move
        transform.position = Vector3.MoveTowards(transform.position, grid.GetCellCenterWorld(next_location), step);
    }

    override
    public string ToString()
    {
        return "Finding Food";
    }
}
