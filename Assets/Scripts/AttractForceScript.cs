using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttractForceScript : MonoBehaviour
{

    public float pullRadius = 10;
    public float pullForce = 5;

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
                collider.GetComponent<Rigidbody>().AddForce(forceDirection.normalized * pullForce * Time.fixedDeltaTime);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
