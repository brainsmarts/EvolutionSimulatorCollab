using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField]
    private Grid grid;
    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;    
    }

    // Update is called once per frame

    public Grid getGrid()
    {
        return grid;
    }

    
}

