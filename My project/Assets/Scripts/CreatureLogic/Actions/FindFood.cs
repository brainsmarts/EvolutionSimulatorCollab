using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class FindFood : ActionBase
{
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

    public void SetScanner(RangeScanner rangeScanner)
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
        Debug.Log("Init");
        Debug.Log("Food Position: " + grid.WorldToCell(food.GetPosition()));
        Debug.Log("Creature Position" + grid.WorldToCell(data.transform.position));
        data.SetNewTargetLocation(grid.WorldToCell(food.GetPosition()));
        running = true;
    }

    public void Run()
    {
        Debug.Log("Run");
        if(food == null){
            running = false;
            //data.SetRandomPath();
            return;
        }

        if(Vector2.Distance(food.GetPosition(), rb.position) <= 0.16f){
            data.IncreaseEnergy(food.EatFood());
            running = false;
            //data.SetRandomPath();
        }
    }

    override
    public string ToString()
    {
        return "Finding Food";
    }
}
