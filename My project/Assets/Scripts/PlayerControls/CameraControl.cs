using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField]
    Camera cam;
    [SerializeField]
    float speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float h_input = Input.GetAxisRaw("Horizontal");
        float v_input = Input.GetAxisRaw("Vertical");

        if(h_input != 0)
        {
            cam.transform.position = new Vector3(transform.position.x + (speed * h_input * Time.deltaTime), transform.position.y, transform.position.z);
        }

        if (v_input != 0)
        {
            cam.transform.position = new Vector3(transform.position.x , transform.position.y + (speed * v_input * Time.deltaTime), transform.position.z);
        }

        float scroll = Input.mouseScrollDelta.y;

        if (scroll > 0 && cam.orthographicSize > .3)
        {
            cam.orthographicSize -= .2f;
        }
        else if (scroll < 0 && cam.orthographicSize < 3)
        {
            cam.orthographicSize += .2f;
        }
    }
}
