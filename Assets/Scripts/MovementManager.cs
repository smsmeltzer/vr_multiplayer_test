using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Photon.Pun;

public class MovementManager : MonoBehaviour
{
    private PhotonView view;
    private GameObject child;
    private float xInput;
    private float yInput;
    private float movementSpeed = 10.0f;

    private InputData inputData;
    private Rigidbody rb;
    private Transform XRrig;

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
    }

    // Update is called once per frame
    void Update()
    {
        if (view.IsMine) {
            XRrig.position = child.transform.position;

            if (inputData.rightController.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 movement))
            {
                //xInput = movement.x;
                //yInput = movement.y;
                Vector3 moveDir = new Vector3(movement.x, 0, movement.y).normalized;
                Vector3 targetMoveAmount = moveDir * movementSpeed;
                moveAmount = Vector3.SmoothDamp(moveAmount, targetMoveAmount, ref smoothMoveVelocity, .15f);
            }
        }
    }

    private void FixedUpdate()
    {
        //rb.AddForce(xInput * movementSpeed, 0, yInput * movementSpeed);
        rb.MovePosition(rb.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);
    }
}
