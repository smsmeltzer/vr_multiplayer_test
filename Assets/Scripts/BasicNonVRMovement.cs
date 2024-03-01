using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Photon.Pun;
using System;

public class BasicNonVRMovement : MonoBehaviourPunCallbacks
{
    private const float CONST_MOVE_SPEED = 10f;

    public float mouseSensitivity = 250f;
    public float moveSpeed = CONST_MOVE_SPEED;
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


    // POWERUP Functionality:
    private bool timer_activated;   
    private float timer_time;

    private int active_powerup;
    private const int POWER_UP_TIME = 5;    // 5 seconds

    [SerializeField] private DisplayRoleScript myUIScript;
    [SerializeField] private AttractForceScript myForceScript;


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

        // POWERUP Functionality:

        timer_time = POWER_UP_TIME;
        timer_activated = false;
        active_powerup = -1;

        myForceScript.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (view.IsMine)
        {
            // POWERUP Functionality:
            if (timer_activated)
            {
                timer_time -= Time.deltaTime;
                if (timer_time <= 0)    // Powerup no longer active, reset all variables
                {
                    timer_time = POWER_UP_TIME;
                    timer_activated = false;
                    myForceScript.enabled = false;
                    moveSpeed = CONST_MOVE_SPEED;
                }


                if (active_powerup == 2)    // Turbo Speed
                {
                    moveSpeed = CONST_MOVE_SPEED + 3;
                }
                else if (active_powerup == 3)   // Attract/Repulse
                {
                    myForceScript.change_force_direction(myUIScript.get_is_tagger());
                    myForceScript.enabled = true;
                }
            }

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

    // Sets active powerup when Player collides with powerup spawner
    public void add_powerup(int powerup)
    {
        active_powerup = powerup;
        timer_activated = true;
    }
}
