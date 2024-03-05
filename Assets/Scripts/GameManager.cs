using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    PhotonView view;
    private bool collisionHandled = false;
    public struct player
    {
        public int id;
        public bool team;
    }
    public int localId;

    public List <player> playerList = new List<player>();
    void Start()
    {
        view = GetComponent<PhotonView>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool addPlayer(int id, bool team)
    {
        if (playerList.Count == 0)
        {
            localId = id;
        }
        for (int i = 0; i < playerList.Count; i++)
        {
            player pl = playerList[i];
            if (pl.id == id) return false;
        }
        player p = new player();
        p.id = id;
        p.team = team;
        playerList.Add(p);
        Debug.Log("player added! " + p.id + " " + p.team);
        return true;
    }


    public void handleTag(int pid, int cid)
    {
        if (!collisionHandled){
            for (int i = 0; i < playerList.Count; i++)
            {
                player pl = playerList[i];
                if (pl.id == pid && pl.team == true)
                {
                    pl.team = false;
                    playerList[i] = pl;
                    //Debug.Log(playerList[i].id + " now has tag status of " + playerList[i].team);
                    for (int j = 0; j < playerList.Count; j++)
                    {
                        player plTwo = playerList[j];
                        if (plTwo.id == cid && plTwo.team == false)
                        {
                            plTwo.team = true;
                            playerList[j] = plTwo;
                            //Debug.Log(playerList[j].id + " now has tag status of " + playerList[j].team);
                            Debug.Log(pl.id + " tagged " + plTwo.id + "successfully");
                        }
                    }
                }
                // else if (pl.id == cid && pl.team == true)
                // {
                //     pl.team = false;
                //     playerList[i] = pl;
                //     Debug.Log(playerList[i].id + " now has tag status of " + playerList[i].team);
                //     for (int j = 0; j < playerList.Count; j++)
                //     {
                //         player plTwo = playerList[j];
                //         if (plTwo.id == pid && plTwo.team == false)
                //         {
                //             plTwo.team = true;
                //             playerList[j] = plTwo;
                //             Debug.Log(playerList[j].id + " now has tag status of " + playerList[j].team);
                //             Debug.Log(pl.id + " tagged " + plTwo.id + "successfully");
                //         }
                //     }
                // }
            }
            StartCoroutine(ResetCollisionFlag());
        }
    }

    public void handleRTag(int pid, int cid)
    {
        if (!collisionHandled){
        for (int i = 0; i < playerList.Count; i++)
        {
            player pl = playerList[i];
            if (pl.id == cid && pl.team == true)
                {
                    pl.team = false;
                    playerList[i] = pl;
                    //Debug.Log(playerList[i].id + " now has tag status of " + playerList[i].team);
                    for (int j = 0; j < playerList.Count; j++)
                    {
                        player plTwo = playerList[j];
                        if (plTwo.id == pid && plTwo.team == false)
                        {
                            plTwo.team = true;
                            playerList[j] = plTwo;
                            //Debug.Log(playerList[j].id + " now has tag status of " + playerList[j].team);
                            Debug.Log(pl.id + " tagged " + plTwo.id + "successfully");
                        }
                    }
                }
            }
            StartCoroutine(ResetCollisionFlag());
        }
    }

    private IEnumerator ResetCollisionFlag()
    {
        collisionHandled = true;
        yield return new WaitForSeconds(5.0f);
        collisionHandled = false;
    }
    // public void addPlayerToAll(int id)
    // {
        
    //     view.RPC("addPlayer", RpcTarget.All, id);
    // }
}
