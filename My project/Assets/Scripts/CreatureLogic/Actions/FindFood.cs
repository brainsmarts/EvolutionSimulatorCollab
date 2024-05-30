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
    private Vector2 next_location;
    private Stack<Vector3Int> path;
    public bool running;

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
            rb.velocity *= 0;
            running = false;
            return;
        }
            
        
        path = GenericMovement.MoveTo(grid.WorldToCell(rb.position), grid.WorldToCell(food.GetPosition()));
        if(path.Count <= 0)
        {
            rb.velocity *= 0;
            running = false;
            return;
        }
        next_location = grid.GetCellCenterWorld(path.Pop());
        running = true;
    }

    public void Run()
    {
        if (Vector2.Distance(rb.position, next_location) < 0.05f)
        {
            if (food == null) {
                rb.velocity *= 0;
                running = false;
                return;
            }
            if (food.InRange(rb))
            {
                int energy_gained = food.EatFood();
                data.IncreaseEnergy(energy_gained);
                running = false;
                rb.velocity *= 0;
                return;
            }
            else
            {
                next_location = grid.GetCellCenterWorld(path.Pop());
                data.DecreaseEnergy(1);
            }
        }
        rb.velocity = new Vector2(next_location.x - rb.position.x, next_location.y - rb.position.y).normalized * data.Speed * Time.deltaTime;
        RaycastHit2D hit = Physics2D.Raycast(rb.position, rb.velocity.normalized, .05f);
        if (hit.collider != null)
        {
            float left = 0;
            if (rb.velocity.x > rb.velocity.y)
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
    }

    override
    public string ToString()
    {
        return "Finding Food";
    }
}
