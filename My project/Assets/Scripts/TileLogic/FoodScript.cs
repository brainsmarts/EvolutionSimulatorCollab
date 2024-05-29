using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private int energy_stored;

    [SerializeField]
    private HashSet<BaseCreature> creatures_in_range;
    public int EatFood()
    {
        Destroy(gameObject);
        return energy_stored;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public bool InRange(Transform creature)
    {
        Collider2D[] results = Physics2D.OverlapCircleAll(transform.position, 0.24f);
        foreach(Collider2D collider in results)
        {
            if (collider.CompareTag("Creature"))
            {
                if(collider.transform == creature)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
