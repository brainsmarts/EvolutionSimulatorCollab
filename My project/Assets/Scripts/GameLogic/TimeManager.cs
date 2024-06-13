using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance {get; private set;}
    private float Timer = 0;
    private float SecondsToDay = 60f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Timer < SecondsToDay){
            Timer += Time.deltaTime;  
        } else {
            
        }
    }
}
