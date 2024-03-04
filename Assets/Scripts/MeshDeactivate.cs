using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Photon.Pun;
using UnityEngine.Animations;

public class MeshDeactivate : MonoBehaviourPunCallbacks
{
    private PhotonView view;
    private GameObject body;
    private GameObject model;
    private GameObject mesh;
    // Start is called before the first frame update
    void Start()
    {
        view = GetComponent<PhotonView>();
        body = transform.GetChild(0).gameObject;
        model = body.transform.GetChild(0).gameObject;
        mesh = model.transform.GetChild(1).gameObject;
        Debug.Log("name: " + mesh.name);
    }

    // Update is called once per frame
    void Update()
    {
        if (view.IsMine)
        {
            mesh.GetComponent<SkinnedMeshRenderer>().enabled = false;
        }
    }
}