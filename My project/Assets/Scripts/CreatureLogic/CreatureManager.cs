using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class CreatureManager : MonoBehaviour
{
    private float LogTimer = 0;
    [SerializeField]
    private float LogRate = 60f;

    public static CreatureManager instance;

    private string file_name = "/Log.txt";

    [SerializeField]
    public TextAsset log;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        
        
    }

    void FixedUpdate(){
        if(LogTimer < LogRate){
            LogTimer += Time.deltaTime;
        }else{
            Debug.Log("Logging State");
            LogTimer = 0;
            LogCreatureStates();
        }
    }

    public void LogCreatureStates(){
        string path = Application.persistentDataPath + file_name;
        StreamWriter writer = new(path, true);
        List<BaseCreature> list_of_creatures = new List<BaseCreature>(gameObject.GetComponentsInChildren<BaseCreature>());
        float av_speed = 0, av_range = 0;
        foreach(BaseCreature creature in list_of_creatures){
            av_range += creature.data.Sight_range;
            av_speed += creature.data.Speed;
        }
        writer.WriteLine("Log");
        writer.WriteLine("Average Speed" + av_speed / list_of_creatures.Count);
        writer.WriteLine("Average Range" + av_range / list_of_creatures.Count);
        writer.Close();
    }
}
