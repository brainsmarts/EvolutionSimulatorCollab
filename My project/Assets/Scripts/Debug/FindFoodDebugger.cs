using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FindFoodDebugger : MonoBehaviour
{
    [SerializeField]
    CreatureData data;
    [SerializeField]
    Rigidbody2D creature_rb;
    [SerializeField]
    RangeScanner scanner;
    ActionBase find_food;
    bool running = false;
    // Start is called before the first frame update
    void Start()
    {
        CreatureData data = new(1, 100, Random.Range(30, 40), 8, new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)), transform);
        find_food = new FindFood();
        find_food.SetData(data);
        find_food.SetRigidBody(creature_rb);
        find_food.SetScanner(scanner);
        scanner.SetRange(data.Sight_range);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)){
            if (running)
            {
                if (find_food.IsRunning())
                {
                    find_food.Run();
                }
                else
                {
                    running = false;
                }
                
                return;
            }
            if (find_food.Condition())
            {
                find_food.Init();
                running = true;
            }
        }
    }
}
