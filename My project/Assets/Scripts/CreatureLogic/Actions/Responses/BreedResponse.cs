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
        breeder_id = data.Target.data.ID;
        grid = GameManager.Instance.getGrid();
        data.DecreaseEnergy(30);
    }

    public bool RunResponse()
    {
        //if creature is in set distance
       
        
        Debug.Log("Breeded");
        return true;
        
        return true;
    }
}
