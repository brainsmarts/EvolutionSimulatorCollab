using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class CreatureManager : MonoBehaviour
{
    private float LogTimer = 0;
    [SerializeField]
    private float LogRate = 60f;

    public static CreatureManager instance;
    private List<BaseCreature> list_of_creatures;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        list_of_creatures = new List<BaseCreature>(gameObject.GetComponentsInChildren<BaseCreature>());
        
    }

    public void AddCreature(BaseCreature creature) 
    {
        list_of_creatures.Add(creature);
    }

    public CreatureData GetData(int creature_id){
        //Debug.Log(creature_id);
        foreach(BaseCreature creature in list_of_creatures)
        {
            if(creature.data.ID == creature_id)
                return creature.data;
        }

        return null;
    }

    void FixedUpdate(){
        if(LogTimer < LogRate){
            LogTimer += Time.deltaTime;
        }else{
            LogCreatureStates();
        }
    }

    public void LogCreatureStates(){

    }
}
