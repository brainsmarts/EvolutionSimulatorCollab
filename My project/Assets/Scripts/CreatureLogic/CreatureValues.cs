using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;

public class CreatureValues
{
    public const int MAX_ENERGY = 0;
    public const int CUR_ENERGY = 1;
    public const int MAX_HEALTH = 2;
    public const int CUR_HEALTH = 3;
    public const int SIGHTRANGE = 4;
    public const int SIZE = 5;

    public static int GetValue(CreatureData data, int value)
    {
        return value switch
        {
            MAX_ENERGY => data.Energy,
            CUR_ENERGY => data.Current_energy,
            MAX_HEALTH => data.Health,
            CUR_HEALTH => data.Health,
            SIGHTRANGE => data.Sight_range,
            SIZE => -1,
            _ => -1,
        };
    }
}
