using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AttractForceScript : MonoBehaviour
{

    private const float CONST_PULL_FORCE = 10;

    private float pullRadius = 15;
    private float pullForce = CONST_PULL_FORCE;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void FixedUpdate()
    {
        foreach (Collider collider in Physics.OverlapSphere(transform.position, pullRadius)) {
            // calculate direction from target to me
            Vector3 forceDirection = transform.position - collider.transform.position;

            // apply force on target towards me
            if (collider.tag == "Player")
            {
                collider.attachedRigidbody.AddForce(forceDirection.normalized * pullForce, ForceMode.Force);
            }
        }
    }

    // Changes force direction based on if the player is a tagger
    public void change_force_direction(bool tagger)
    {
        if (tagger)
        {
            pullForce = CONST_PULL_FORCE;
        }
        else
        {
            pullForce = -CONST_PULL_FORCE;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
