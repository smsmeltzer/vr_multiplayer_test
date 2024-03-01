using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private Vector3 myStartPosition;

    private bool timer_activated;
    private float timer_time;

    private int active_powerup;
    private const int POWER_UP_TIME = 5;    // 5 seconds

    [SerializeField] private DisplayRoleScript myUIScript;
    [SerializeField] private AttractForceScript myForceScript;

    // Start is called before the first frame update
    void Start()
    {
        myStartPosition = transform.position;

        timer_time = POWER_UP_TIME;
        timer_activated = false;
        active_powerup = -1;

        myForceScript.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        float x = GetComponent<Rigidbody>().velocity.x;
        float y = GetComponent<Rigidbody>().velocity.y;
        float z = GetComponent<Rigidbody>().velocity.z;

        float moveSpeed = 5;
        
        if(timer_activated)
        {
            timer_time -= Time.deltaTime;
            if (timer_time <= 0)
            {
                timer_time = POWER_UP_TIME;
                timer_activated = false;
                myForceScript.enabled = false;
            }
            else if (active_powerup == 2)
            {
                moveSpeed = 7;
            }
            else if (active_powerup == 3)
            {
                myForceScript.change_force_direction(myUIScript.get_is_tagger());
                myForceScript.enabled = true;
            }
        }

        if (Input.GetKey("left shift"))
        {
            moveSpeed = 8;
        } else
        {
            moveSpeed = 5;
        }

        if(Input.GetKeyDown("space") && (GetComponent<Rigidbody>().velocity.y == 0))
        {
            y = 5;
        }

        if (Input.GetKey("up") || Input.GetKey("w"))
        {
            z = moveSpeed;
        }

        if (Input.GetKey("down") || Input.GetKey("s"))
        {
            z = -moveSpeed;
        }

        if (Input.GetKey("left") || Input.GetKey("a"))
        {
            x = -moveSpeed;
        }

        if (Input.GetKey("right") || Input.GetKey("d"))
        {
            x = moveSpeed;
        }

        if (Input.GetKey("r") || (transform.position.y < -50))
        {
            transform.position = myStartPosition;
            x = 0;
            y = 0;
            z = 0;
        }

        GetComponent<Rigidbody>().velocity = new Vector3(x, y, z);
    }

    private void OnCollisionEnter(Collision other)
    {
        if(c.gameObject.tag == "testObj")
        {
            myUIScript.lose_life();
            myUIScript.change_role();
            myForceScript.change_force_direction(myUIScript.get_is_tagger());

        }
    }

    public void add_powerup(int powerup)
    {
        active_powerup = powerup;
        timer_activated = true;
    }
}
