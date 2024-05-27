using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CreatureManager : MonoBehaviour
{

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

    public void RemoveCreature(BaseCreature creature){
        BaseCreature remove = creature;
        /*
        foreach (BaseCreature c in list_of_creatures)
        {
            if (c.GetID() == creature.GetID()){
                remove = c;
                break;
            }
        }*/

        list_of_creatures.Remove(remove);
    }

    public void FixedUpdate()
    {
    }
    public List<int> GetCreatureInRange(Vector3Int position, int range)
    {
        List<int> creatures_in_range = new List<int>();
        foreach(BaseCreature creature in list_of_creatures)
        {
            if (Vector3Int.Distance(position, GameManager.Instance.getGrid().WorldToCell(creature.GetPosition())) <= range)
            {
                creatures_in_range.Add(creature.GetID()); 
            }
        }
        return creatures_in_range;
    }

    public Dictionary<int,int> GetCreaturesInRange(Vector3Int position, int range, int value)
    {
        Dictionary<int, int> creatures = new Dictionary<int, int>();
        foreach (BaseCreature creature in list_of_creatures)
        {
            if (Vector3Int.Distance(position, GameManager.Instance.getGrid().WorldToCell(creature.GetPosition())) <= range)
            {
                creatures.Add(creature.GetID(), CreatureValues.GetValue(creature.data, value));
            }
        }
        return creatures;
    }

    public Vector3Int GetCreaturePosition(int creature_id)
    {
        BaseCreature creature = GetCreature(creature_id);
        return GameManager.Instance.getGrid().WorldToCell(creature.GetPosition());
    }

    public int GetCreatureAt(Vector3Int position)
    {
        foreach(BaseCreature creature in list_of_creatures)
        {
            Vector3Int creature_position = GameManager.Instance.getGrid().WorldToCell(creature.GetPosition());
            if (creature_position.x == position.x && creature_position.y == position.y)
            {
                return creature.GetID();
            }
        }

        return -1;
    }
    
    public BaseCreature GetCreature(int creature_id)
    {
        foreach(BaseCreature creature in list_of_creatures)
        {
            if(creature.GetID() == creature_id)
                return creature;
        }

        return null;
    }

    public CreatureData GetData(int creature_id){
        //Debug.Log(creature_id);
        foreach(BaseCreature creature in list_of_creatures)
        {
            if(creature.GetID() == creature_id)
                return creature.data;
        }

        return null;
    }

    public bool SendRequest(int request_id, int creature_id)
    {
        BaseCreature recipient = GetCreature(creature_id);
        bool response = recipient.SendRequest(request_id, creature_id);
        return response;
    }

    public List<BaseCreature> GetCreatures(){
        return list_of_creatures;
    }
}
