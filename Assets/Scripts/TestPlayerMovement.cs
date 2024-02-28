using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private Vector3 myStartPosition;

    // Start is called before the first frame update
    void Start()
    {
        myStartPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float x = GetComponent<Rigidbody>().velocity.x;
        float y = GetComponent<Rigidbody>().velocity.y;
        float z = GetComponent<Rigidbody>().velocity.z;

        float moveSpeed = 0;
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
    }
}
