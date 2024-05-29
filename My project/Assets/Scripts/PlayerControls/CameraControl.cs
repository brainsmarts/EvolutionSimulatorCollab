using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField]
    Camera cam;
    [SerializeField]
    float speed;
    [SerializeField]
    private Transform following;
    [SerializeField]
    private bool is_follwing = false;

    public static CameraControl Instance { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (following == null)
            is_follwing = false;

        if(is_follwing == false)
        {
            PlayerControl();
        } else
        {
            FollowCreature();
        }
        Zoom();
    }

    private void PlayerControl()
    {
        float h_input = Input.GetAxisRaw("Horizontal");
        float v_input = Input.GetAxisRaw("Vertical");

        if (h_input != 0)
        {
            cam.transform.position = new Vector3(transform.position.x + (speed * h_input * (Time.deltaTime/Time.timeScale)), transform.position.y, transform.position.z);
        }

        if (v_input != 0)
        {
            cam.transform.position = new Vector3(transform.position.x, transform.position.y + (speed * v_input * (Time.deltaTime / Time.timeScale)), transform.position.z);
        }
    }

    public void SetFollow(Transform creature)
    {
        is_follwing = true;
        following = creature; 
    }

    public void StopFollow()
    {
        is_follwing = false;
    }
    private void FollowCreature()
    {

        cam.transform.position = new Vector3(following.position.x, following.position.y, transform.position.z);
    }


    private void Zoom()
    {
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
