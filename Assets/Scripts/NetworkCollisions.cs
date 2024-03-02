using UnityEngine;
using System.Collections;

public class NetworkCollisions : MonoBehaviour
{
    bool hasCollided;
    private bool isIt;
    float collisionCooldown = 5.0f;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Body" && !hasCollided)
        {
            //Debug.Log("Collision detected with: " + collision.gameObject.name);
            
            isIt = transform.parent.transform.GetChild(1).gameObject.GetComponent<DisplayRoleScript>().get_is_tagger();
            Debug.Log("isIt = " + isIt);
            hasCollided = true;
            if (isIt)
            {
                //isIt = false;
                //collision.gameObject.GetComponent<NetworkCollisions>().isIt = true;
                transform.parent.transform.GetChild(1).gameObject.GetComponent<DisplayRoleScript>().change_role();
                GameObject colUI = collision.gameObject.transform.parent.transform.GetChild(1).gameObject;
                Debug.Log("collision role = " + colUI.GetComponent<DisplayRoleScript>().get_is_tagger());
                colUI.GetComponent<DisplayRoleScript>().change_role();
                colUI.GetComponent<DisplayRoleScript>().lose_life();
                Debug.Log("collision role = " + colUI.GetComponent<DisplayRoleScript>().get_is_tagger());
                FreezePlayer(collision.gameObject.GetComponent<Rigidbody>());
            }
            StartCoroutine(ResetCollisionFlag());
            StartCoroutine(UnfreezePlayer(collision.gameObject.GetComponent<Rigidbody>()));

        }
    }
    private IEnumerator ResetCollisionFlag()
    {
        //Debug.Log("countdown begin");
        yield return new WaitForSeconds(collisionCooldown);
        //Debug.Log("countdown end");
        hasCollided = false;
    }
    private void FreezePlayer(Rigidbody rb)
    {
        rb.constraints = RigidbodyConstraints.FreezePosition;
    }

    IEnumerator UnfreezePlayer(Rigidbody rb)
    {
        yield return new WaitForSeconds(collisionCooldown);

        rb.constraints = RigidbodyConstraints.None;
    }

}


