using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Photon.Pun;

public class PlayerRoleManager : MonoBehaviourPunCallbacks
{
    private bool team = false;
    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1 )
        {
            team = true;
        }
        Debug.Log(team);
    }

    public void ChangeTeam()
    {
        team = !team;
    }
}
