using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreedResponse : Response
{
    private int breeder_id;
    private Rigidbody2D rb;
    private CreatureData data;
    private Grid grid;
    RangeScanner scanner;
    public BreedResponse(Rigidbody2D rb, CreatureData data, RangeScanner scanner)
    {
        this.rb = rb;
        this.data = data;
        this.scanner = scanner;
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
       
        
        Debug.Log("Breeded by:" + data.Target.data.ID);

        return false;
    }
}
