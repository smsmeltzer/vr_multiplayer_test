using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ RequireComponent(typeof (Rigidbody))]
public class GravityBody : MonoBehaviour
{
    GravityAttractor planet;

    // Start is called before the first frame update
    void Start()
    {
        planet = GameObject.FindGameObjectWithTag("Planet").GetComponent<GravityAttractor>();
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
    }

    // Update is called once per frame
    void Update()
    {
        planet.Attract(GetComponent<Transform>());
    }
}
