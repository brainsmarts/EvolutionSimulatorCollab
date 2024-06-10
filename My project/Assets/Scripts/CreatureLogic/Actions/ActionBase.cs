using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ActionBase 
{
    public void SetRigidBody(Rigidbody2D rb);
    public void SetData(CreatureData data);
    public void SetScanner(ref RangeScanner scanner);
    public bool Condition();

    public bool IsRunning();

    // Update is called once per frame
    public void Init();
    public void Run();

    public string ToString();
}
