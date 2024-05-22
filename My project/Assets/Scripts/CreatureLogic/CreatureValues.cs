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
        switch (value)
        {
            case MAX_ENERGY:
                return data.energy;
            case CUR_ENERGY:
                return data.energy;
            case MAX_HEALTH:
                return data.health;
            case CUR_HEALTH:
                return data.health;
            case SIGHTRANGE:
                return data.sight_range;
            case SIZE:
                return -1;
        }
        return -1;
    }
}
