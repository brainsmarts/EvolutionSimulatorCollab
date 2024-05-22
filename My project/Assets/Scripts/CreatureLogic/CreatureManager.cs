using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CreatureManager : MonoBehaviour
{
    public static CreatureManager instance;
    private List<BaseCreature> list_of_creatures;

    [SerializeField]
    TextMeshProUGUI debug_text;
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

    public void FixedUpdate()
    {
        string display = "";
        foreach (BaseCreature creature in list_of_creatures)
        {
            display += creature.GetID() + " " + creature.GetCurrentAction() + "\n";
        }
        debug_text.text = display;
    }
    public List<int> GetCreatureInRange(Vector3Int position, int range)
    {
        List<int> creatures_in_range = new List<int>();
        foreach(BaseCreature creature in list_of_creatures)
        {
            if (Vector3Int.Distance(position, GameManager.Instance.getGrid().WorldToCell(creature.GetGridPosition())) <= range)
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
            if (Vector3Int.Distance(position, GameManager.Instance.getGrid().WorldToCell(creature.GetGridPosition())) <= range)
            {
                creatures.Add(creature.GetID(), CreatureValues.GetValue(creature.GetStats(), value));
            }
        }
        return creatures;
    }

    public Vector3Int GetCreaturePosition(int creature_id)
    {
        BaseCreature creature = GetCreature(creature_id);
        return GameManager.Instance.getGrid().WorldToCell(creature.GetGridPosition());
    }
    
    private BaseCreature GetCreature(int creature_id)
    {
        foreach(BaseCreature creature in list_of_creatures)
        {
            if(creature.GetID() == creature_id)
                return creature;
        }

        return null;
    }

    public bool SendRequest(int request_id, int creature_id)
    {
        BaseCreature recipient = GetCreature(creature_id);
        bool response = recipient.SendRequest(request_id, creature_id);
        return response;
    }
}
