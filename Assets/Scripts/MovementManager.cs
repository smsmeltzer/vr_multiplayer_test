using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Photon.Pun;
using UnityEngine.Animations;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using Unity.XR.CoreUtils;

public class MovementManager : MonoBehaviourPunCallbacks
{
    private const float CONST_MOVE_SPEED = 10f; // constant movespeed
    private const int POWER_UP_TIME = 5;    // 5 sec is duration of a powerup


    private PhotonView view;
    private GameObject child;
    private float xInput;
    private float yInput;
    public float moveSpeed = CONST_MOVE_SPEED;
    public float jumpSpeed = 10.0f;
    public LayerMask groundedMask;

    private bool grounded = false;

    private InputData inputData;
    private Rigidbody rb;
    private Transform XRrig;
    private Transform cameraT;

    Vector3 moveAmount;
    Vector3 smoothMoveVelocity;

    private XRController RightController;
    private XRRayInteractor rayInteractor;
    public InputHelpers.Button button;

    // Timer for powerup duration
    private bool timer_activated;
    private float timer_time;

    private float tp_timer = 1;

    private int stored_tps;

    private DisplayRoleScript myUIScript;
    [SerializeField] private AttractForceScript myForceScript;  

    // Start is called before the first frame update
    void Start()
    {
        view = GetComponent<PhotonView>();

        child = transform.GetChild(0).gameObject;
        rb = child.GetComponent<Rigidbody>();
        GameObject XrOrigin = GameObject.Find("XR Origin");

        XRrig = XrOrigin.transform;
        inputData = XrOrigin.GetComponent<InputData>();
        cameraT = XrOrigin.GetComponentInChildren<Camera>().transform;

        RightController = XrOrigin.transform.Find("Camera Offset").Find("Right Controller").GetComponent<XRController>();
        rayInteractor = XrOrigin.transform.Find("Camera Offset").Find("Right Controller").GetComponent<XRRayInteractor>();

        myUIScript = XrOrigin.transform.Find("Camera Offset").Find("Right Controller").Find("UI").GetComponent<DisplayRoleScript>();

        if (!view.IsMine)
        {
            cameraT.gameObject.SetActive(false);
        }

        if (view.IsMine)
        {
            timer_time = POWER_UP_TIME;
            timer_activated = false;
            stored_tps = 0;

            myForceScript.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (view.IsMine) {
            if (timer_activated) 
            {
                timer_time -= Time.deltaTime;
                if (timer_time <= 0)    // Powerup timer ended, reset all variables
                {
                    timer_time = POWER_UP_TIME;
                    timer_activated = false;
                    myForceScript.enabled = false;
                    moveSpeed = CONST_MOVE_SPEED;
                }
            }

            if (tp_timer <= 0)
            {
                bool pressed;
                RightController.inputDevice.IsPressed(button, out pressed);

                if (pressed && stored_tps != 0)
                {
                    rb.MovePosition(rayInteractor.rayEndPoint);
                    stored_tps--;
                    tp_timer = 1;
                    myUIScript.use_tp();
                }
            }
            else
            {
                tp_timer -= Time.deltaTime;
            }

            XRrig.position = child.transform.position;
            Look();
            Move();

        }
    }

    void Move()
    {
        if (inputData.rightController.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 movement))
        {
            Vector3 moveDir = new Vector3(movement.x, 0, movement.y).normalized;
            Vector3 targetMoveAmount = moveDir * moveSpeed;
            moveAmount = Vector3.SmoothDamp(moveAmount, targetMoveAmount, ref smoothMoveVelocity, .15f);
        }
        if (inputData.rightController.TryGetFeatureValue(CommonUsages.primaryButton, out bool jump))
        {
            if (grounded)
            {
                rb.AddForce(child.transform.up * jumpSpeed);
            }
        }

        grounded = false;
        Ray ray = new Ray(child.transform.position, -child.transform.up);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1 + .1f, groundedMask))
        {
            grounded = true;
        }
    }

    void Look()
    {
        XRrig.up = child.transform.up;
        child.transform.forward = cameraT.forward;
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + child.transform.TransformVector(moveAmount) * Time.fixedDeltaTime);
    }

    // Called by Powerup script to enable powerup effects on player 
    public void add_powerup(int powerup)
    {
        if (powerup == 1)   // teleportation
        {
            stored_tps += 1;
            myUIScript.add_tp();
        }
        else if (powerup == 2)    // Turbo speed

        {
            timer_activated = true; // start powerup timer
            moveSpeed = CONST_MOVE_SPEED + 3;
        }
        else if (powerup == 3)   // Attract/Repulse
        {
            timer_activated = true; // start powerup timer
            myForceScript.change_force_direction(myUIScript.get_is_tagger());   // Accesses UI script to get role
            myForceScript.enabled = true;
        }
    }
}
