using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreedResponse : Response
{
    private int breeder_id;
    private Transform transform;
    private CreatureData data;
    private Grid grid;
    public BreedResponse(Transform transform, CreatureData data)
    {
        this.transform = transform;
        this.data = data;
    }
    public void Init()
    {
        breeder_id = data.Target_id;
        grid = GameManager.Instance.getGrid();
        data.DecreaseEnergy(30);
    }

    public bool RunResponse()
    {
        if (CreatureManager.instance.GetCreaturePosition(breeder_id).Equals(grid.WorldToCell(transform.position)))
        {
            Debug.Log("Breeded");
            return false;
        }
        return true;
    }
}
