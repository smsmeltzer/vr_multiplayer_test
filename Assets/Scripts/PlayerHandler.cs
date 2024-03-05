using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerHandler : MonoBehaviourPunCallbacks//, IPunInstantiateMagicCallback
{
    // Start is called before the first frame update
    GameManager gameManager;
    PhotonView view;
    bool team;
    int id;
    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        view = GetComponent<PhotonView>();
        
        id = view.ViewID;
        //Debug.Log(id);
        if (id == 1001)
        {
            //Debug.Log("player count " + PhotonNetwork.CountOfPlayers);
            team = true;
        }
        else
        {
            team = false;
        }
        
        view.RPC("addPlayer", RpcTarget.All, id, team);
        // if (view.IsMine){
        //     gameManager.GetComponent<GameManager>().addPlayer(view.ViewID);
        // }
    }

    [PunRPC]
    public void addPlayer (int pid, bool pteam)
    {
        //if(photonView.IsMine)
        //{
            if (gameManager != null){
                if (gameManager.addPlayer(pid, pteam))
                {
                    photonView.RPC("addPlayer", RpcTarget.All, id, team);
                }
            }
            
        //}
        
    }

    public void ttag(int pid, int cid)
    {
        photonView.RPC("tagRPC", RpcTarget.All, pid, cid);
    }

    [PunRPC]
    public void tagRPC (int pid, int cid)
    {
        if (photonView.IsMine)
        {
            gameManager.handleTag(pid, cid);
        }
    }

    public void rtag(int pid, int cid)
    {
        photonView.RPC("rtagRPC", RpcTarget.All, pid, cid);
    }

    [PunRPC]
    public void rtagRPC (int pid, int cid)
    {
        if (photonView.IsMine)
        {
            gameManager.handleRTag(pid, cid);
        }
    }

}
