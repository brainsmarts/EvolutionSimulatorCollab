using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Response 
{
    public static Response GetResponse(int action_id, Rigidbody2D rb, CreatureData data, RangeScanner scanner)
    {
        switch (action_id)
        {
            case 100: // breed request
                Response response = new BreedResponse(rb, data, scanner);
                return response;

            case 101:
                break;
        }
        return null;
    }

    public static bool Reply(int action_id, CreatureData data)
    {
        switch (action_id)
        {  
            case 100:
                //add in some breeding modifier if they have one
                if (data.Current_energy > 30 && Time.time - data.TimeBorn > 60)
                {
                    return true;
                }
                break;
        }
        return false;
    }
    void Init();
    bool RunResponse();
}
