using Photon.Pun;
using System.Collections;
using UnityEngine;
using System;


public class TaggingScript : MonoBehaviour
{
    private int startingLives = 3;
    private int currentLives;
    private bool collisionCooldownActive = false;
    private float collisionCooldownDuration = 5f;
    private bool currentlyIt;

    private PhotonView myView;


    private void Start()
    {
        myView = transform.parent.gameObject.GetComponent<PhotonView>();
        Debug.Log(myView.ViewID);
        if (myView.IsMine)
        {
            currentLives = startingLives;
            if (PhotonNetwork.CountOfPlayers == 1)
            {
                Debug.Log("Player " + PhotonNetwork.CountOfPlayers + " is it and has id " + myView.ViewID);
                currentlyIt = true;
            }
            else
            {
                Debug.Log("Player " + PhotonNetwork.CountOfPlayers + " is not it");
                currentlyIt = false;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collisionCooldownActive && collision.gameObject.name == "Body") 
        {
            int pvid = collision.gameObject.GetComponent<TaggingScript>().getMyView().ViewID;
            bool beingTagged = false;
            if (currentlyIt)
            {
                beingTagged = true;
            }
            // Debug.Log(collision.gameObject.GetPhotonView().gameObject.name + " has id " + collision.gameObject.GetPhotonView().ViewID);
            // if (collision.gameObject.name == "Body")
            // {
            //     Debug.Log("Collided with " + collision.gameObject.GetComponent<TaggingScript>().getMyView().ViewID);
            //     if (currentlyIt)
            //     {
            //         Debug.Log(myView.ViewID + " is tagging " + collision.gameObject.GetComponent<TaggingScript>().getMyView().ViewID);
                        myView.RPC("TagPlayer", RpcTarget.All, pvid, beingTagged);
            //         currentlyIt = false;
            //         Debug.Log(myView.ViewID + " currentlyIt is now " + currentlyIt);
            //     }
            // }
            StartCoroutine(ActivateCollisionCooldown());
        }
    }

    private IEnumerator ActivateCollisionCooldown()
    {
        collisionCooldownActive = true;
        yield return new WaitForSeconds(collisionCooldownDuration);
        collisionCooldownActive = false;
    }

    [PunRPC]
    void TagPlayer(int pvid, bool beingTagged)
    {
        if (myView.IsMine)
        {
            if (currentlyIt)
            {
                currentlyIt = false;
            }

            else if (myView.ViewID == pvid && beingTagged)
            {
                LoseLife();
                currentlyIt = true;
                FreezePlayer(collisionCooldownDuration);
            }
        // Debug.Log("Tagging");
        // LoseLife();
        // ChangeRoleToTagged();
        // FreezePlayer(collisionCooldownDuration);
        }
    }

    private void LoseLife()
    {
        currentLives--;
        if (currentLives <= 0)
        {
            Debug.Log("Game Over");
        }
        // Update UI or other actions related to life loss
    }

    private void ChangeRoleToTagged()
    {
        currentlyIt = true;
        Debug.Log(myView.ViewID + " currentlyIt is now " + currentlyIt);
        // Implement role change to tagged and update UI if necessary
    }

    private void FreezePlayer(float duration)
    {
        StartCoroutine(FreezeCoroutine(duration));
    }

    private IEnumerator FreezeCoroutine(float duration)
    {
        Rigidbody rb = GetComponent<Rigidbody>(); // Assuming the player has a Rigidbody
        rb.constraints = RigidbodyConstraints.FreezeAll;
        yield return new WaitForSeconds(duration);
        rb.constraints = RigidbodyConstraints.None;
    }

    public PhotonView getMyView()
    {
        return myView;
    }

    public bool getCurrentlyIt ()
    {
        return currentlyIt;
    }
}
