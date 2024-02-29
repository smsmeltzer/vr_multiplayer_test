using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Photon.Pun;
using System;

public class BasicNonVRMovement : MonoBehaviourPunCallbacks
{
    public float mouseSensitivity = 250f;
    public float moveSpeed = 10f;
    public float maxPitch = 85f;
    public float minPitch = -85f;
    public float gravity = 15f;
    public float jumpSpeed = 10f;
    public float tol = 0.01f;

    private float pitch = 0f;
    private float yVelocity = 0f;

    Transform cameraT;
    float verticalLookRotation;

    Vector3 moveAmount;
    Vector3 smoothMoveVelocity;

    private PhotonView view;
    private GameObject child;
    private Rigidbody rb;

    private CharacterController cc;
    private GameObject planet;

    // Start is called before the first frame update
    void Start()
    {
        view = GetComponent<PhotonView>();
        planet = GameObject.FindGameObjectWithTag("Planet");
        
        cameraT = GetComponentInChildren<Camera>().transform;
        child = transform.GetChild(0).gameObject;
        rb = child.GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        
        if (!view.IsMine)
        {
            cameraT.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (view.IsMine)
        {
            Look();
            Move();
        }
    }

    void Look()
    {
        //get the mouse inpuit axis values
        float xInput = Input.GetAxis("Mouse X") * mouseSensitivity;
        float yInput = Input.GetAxis("Mouse Y") * mouseSensitivity;
        //turn the whole object based on the x input
        child.transform.Rotate(0, xInput, 0);
        //now add on y input to pitch, and clamp it
        pitch -= yInput;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
        //create the local rotation value for the camera and set it
        Quaternion rot = Quaternion.Euler(pitch, 0, 0);
        cameraT.localRotation = rot;
    }

    void Move()
    {
        Vector3 moveDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
        Vector3 targetMoveAmount = moveDir * moveSpeed;
        moveAmount = Vector3.SmoothDamp(moveAmount, targetMoveAmount, ref smoothMoveVelocity, .15f);
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + child.transform.TransformVector(moveAmount) * Time.fixedDeltaTime);
    }
}
