using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindFood : ActionBase
{
    public int weight { get; }
    public Transform transform;
    public CreatureData data;
    private Grid grid;
    private Vector3Int food_location;
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
        food_location = FoodManager.Instance.FoodInRange(grid.WorldToCell(transform.position), data.Sight_range);
        return !food_location.Equals(grid.WorldToCell(transform.position));

    }


    public void Init()
    {
        //List<Vector3Int> neighboors = GenericMovement.GetNeighboors(grid.WorldToCell(transform.position));
        path = GenericMovement.MoveTo(grid.WorldToCell(transform.position),food_location);
        next_location = path.Pop();
        running = true;
    }

    public void Run()
    {
        if (Vector3.Distance(transform.position, grid.GetCellCenterWorld(next_location)) < 0.01f)
        {
            if (grid.WorldToCell(transform.position).Equals(food_location))
            {
                int energy_gained = FoodManager.Instance.EatFood(food_location);
                data.IncreaseEnergy(energy_gained);
                running = false;
            }
            else
            {
                next_location = path.Pop();
                data.DecreaseEnergy(1);
            }
        }
        var step = 0.5f * Time.deltaTime; // calculate distance to move
        transform.position = Vector3.MoveTowards(transform.position, grid.GetCellCenterWorld(next_location), step);
    }

    override
    public string ToString()
    {
        return "Finding Food";
    }
}
