using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityAttractor : MonoBehaviour
{

    public float gravity = -10;

    public void Attract(Transform body)
    {
        Vector3 direction = (body.position - transform.position).normalized;
        
        body.rotation = Quaternion.FromToRotation(body.up, direction) * body.rotation;
        body.GetComponent<Rigidbody>().AddForce(direction * gravity);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
