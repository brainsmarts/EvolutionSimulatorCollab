using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WASDCollisionTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal"); // A/D or Left/Right arrow keys
        float moveVertical = Input.GetAxis("Vertical"); // W/S or Up/Down arrow keys

        Vector3 movement = new Vector3(moveHorizontal, moveVertical);
        transform.Translate(movement * 5 * Time.deltaTime, Space.World);
    }
}
