using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : ActionBase
{
    public int weight => throw new System.NotImplementedException();

    public bool Condition()
    {
        return true;
    }

    public void Init()
    {

    }

    public bool IsRunning()
    {
        throw new System.NotImplementedException();
    }

    public void Run()
    {
        throw new System.NotImplementedException();
    }

    public void SetData(CreatureData data)
    {
        throw new System.NotImplementedException();
    }

    public void SetRigidBody(Rigidbody2D rb)
    {
        throw new System.NotImplementedException();
    }

    public void SetScanner(ref RangeScanner scanner)
    {
        throw new System.NotImplementedException();
    }
}
