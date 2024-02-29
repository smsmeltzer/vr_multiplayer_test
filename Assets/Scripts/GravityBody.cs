using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class GravityBody : MonoBehaviourPunCallbacks
{
    private GravityAttractor planet;
    private GameObject child;
    private Rigidbody rb;
    private PhotonView view;

    // Start is called before the first frame update
    void Start()
    {
        view = GetComponent<PhotonView>();
        planet = GameObject.FindGameObjectWithTag("Planet").GetComponent<GravityAttractor>();

        child = transform.GetChild(0).gameObject;
        rb = child.GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (view.IsMine)
        {
            planet.Attract(child.GetComponent<Transform>());
        }
    }
}
