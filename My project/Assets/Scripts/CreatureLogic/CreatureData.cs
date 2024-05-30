using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]

public class CreatureData
{
    [SerializeField]
    public int ID {get;}
    [SerializeField]
    public int Energy {get;}
    [SerializeField]
    public int Current_energy {get; private set;}
    [SerializeField]
    public int Health {get;}
    [SerializeField]
    public int Sight_range {get;}
    [SerializeField]
    public int Speed { get; private set;}
    public BaseCreature Target {get; set;}
    public int Request_id {get; set;}

    public float TimeBorn {get; private set;}
    public List<ActionBase> Actions {get; private set;}
    public Color Color { get; private set;}

    public CreatureData(int ID, int energy, int speed, int sight_range, Color color)
    {
        this.ID = ID;
        this.Energy = energy;
        this.Speed = speed;
        this.Sight_range = sight_range;
        this.Actions = Actions;
        Current_energy = energy / 2;
        TimeBorn = Time.time;
        Color = color;
    }

    public void SetActions(List<ActionBase> actions){
        this.Actions = actions;
    }

    public void DecreaseEnergy(int amount){
        //Debug.Log("Decreasing Energy" + ID);
        Current_energy -= amount;
    }

    public void IncreaseEnergy(int amount){
        Current_energy += amount;
        if (Current_energy > Energy) {
            Current_energy = Energy;
        }
    }
    public bool IsFull()
    {
        return Current_energy >= Energy;
    }
}
