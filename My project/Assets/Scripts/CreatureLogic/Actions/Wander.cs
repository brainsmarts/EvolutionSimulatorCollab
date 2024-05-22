using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wander : ActionBase
{

    public int weight { get; }
    public Transform transform;
    public CreatureData stats;
    private Grid grid;
    private Vector3Int new_position;
    public bool running;

    public Wander()
    {
        weight = 0;
        grid = GameManager.Instance.getGrid();
    }

    public void SetTransform(Transform transform)
    {
        this.transform = transform;
    }

    public void SetData(CreatureData stats)
    {
        this.stats = stats;
    }

    public bool IsRunning()
    {
        return running; 
    }

    public bool Condition()
    {
        return true;
    }

    public void DoAction(Transform transform, CreatureData stats)
    {
        
    }

    public void Init()
    {
        List <Vector3Int> neighboors = GenericMovement.GetNeighboors(grid.WorldToCell(transform.position));
        int random = Random.Range(0, neighboors.Count); 
        new_position = neighboors[random];
        running = true;
    }

    public void Run()
    {
        if (Vector3.Distance(transform.position, grid.GetCellCenterWorld(new_position)) < 0.01f)
        {
            running = false;
        }
        var step = 0.5f * Time.deltaTime; // calculate distance to move
        transform.position = Vector3.MoveTowards(transform.position, grid.GetCellCenterWorld(new_position), step);
    }

    override
    public string ToString()
    {
        return "Wandring";
    }
}
