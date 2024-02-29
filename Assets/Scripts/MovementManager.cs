using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Photon.Pun;
using UnityEngine.Animations;
using UnityEngine.UI;

public class MovementManager : MonoBehaviourPunCallbacks
{
    private PhotonView view;
    private GameObject child;
    private float xInput;
    private float yInput;
    public float moveSpeed = 10.0f;
    public float jumpSpeed = 10.0f;
    public LayerMask groundedMask;

    private bool grounded = false;

    private InputData inputData;
    private Rigidbody rb;
    private Transform XRrig;
    private Transform cameraT;

    Vector3 moveAmount;
    Vector3 smoothMoveVelocity;
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

        if (!view.IsMine)
        {
            cameraT.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (view.IsMine) {
            Look();
            Move();
        }
    }

    void Move()
    {
        XRrig.position = child.transform.position;

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
        child.transform.rotation = Quaternion.LookRotation(cameraT.forward);
        XRrig.transform.rotation = Quaternion.LookRotation(child.transform.forward, child.transform.up);
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + child.transform.TransformVector(moveAmount) * Time.fixedDeltaTime);
    }
}
