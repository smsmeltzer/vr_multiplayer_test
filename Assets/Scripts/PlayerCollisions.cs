using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class PlayerCollisions : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    int pid;
    int cid;
    PhotonView view;
    PhotonView cview;
    bool hasCollided;
    //List <player> pList;

    private void OnCollisionEnter(Collision collision)
    {
        if (!hasCollided && collision.gameObject.name == "Body")
        {
            if (photonView.IsMine)
            {
                // pList = GameObject.Find("GameManager").GetComponent<GameManager>().playerList;
                // player p = pList.Find(p => p.team == true);
                view = GetComponentInParent<PhotonView>();
                cview = collision.gameObject.GetComponentInParent<PhotonView>();
                pid = view.ViewID;
                cid = cview.ViewID;
                //Debug.Log("Collision between " + pid + " and " + cid);
                if (GameObject.Find("GameManager").GetComponent<GameManager>().playerList.Find(p => p.team == true).id == pid){
                GetComponentInParent<PlayerHandler>().ttag(pid, cid);
                }
                else if (GameObject.Find("GameManager").GetComponent<GameManager>().playerList.Find(p => p.team == true).id == cid){
                GetComponentInParent<PlayerHandler>().rtag(pid, cid);
                }
                StartCoroutine(ResetCollisionFlag());
            }
        }
    }

    private IEnumerator ResetCollisionFlag()
    {
        yield return new WaitForSeconds(5.0f);
        hasCollided = false;
    }
}
