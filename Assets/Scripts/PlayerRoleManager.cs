using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Photon.Pun;

public class PlayerRoleManager : MonoBehaviourPunCallbacks
{
    private bool team = false;
    void Start()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2 )
        {
            team = true;
        }

        Debug.Log(team);
    }
}
