using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class FindFood : ActionBase
{
    public int weight { get; }
    public Rigidbody2D rb;
    public CreatureData data;
    private RangeScanner scanner;
    private Grid grid;
    private FoodScript food;
    public bool running = false;

    public bool IsRunning(){
        return running;
    }

    public FindFood()
    {
        weight = 1;
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

    public void SetScanner(ref RangeScanner rangeScanner)
    {
        
        scanner = rangeScanner;
    }

    public bool Condition()
    {
        if (data.IsFull())
        {
            return false;
        }
        food = scanner.GetNearestFood();  
        return food != null;
    }


    public void Init()
    {
        data.SetNewTargetLocation(grid.WorldToCell(food.GetPosition()));
        running = true;
        Debug.Log("Food Init");
    }

    public void Run()
    {
        if(food == null){
            running = false;
            data.SetRandomPath();
        }

        if(Vector2.Distance(food.GetPosition(), rb.position) < .05){
            data.IncreaseEnergy(food.EatFood());
            running = false;
            data.SetRandomPath();
        }

        //Debug.Log("Find Food Run");
    }

    override
    public string ToString()
    {
        return "Finding Food";
    }
}
