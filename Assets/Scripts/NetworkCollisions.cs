using UnityEngine;
using System.Collections;

public class NetworkCollisions : MonoBehaviour
{
    bool hasCollided;
    public bool isIt;
    float collisionCooldown = 5.0f;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Body" && !hasCollided)
        {
            //Debug.Log("Collision detected with: " + collision.gameObject.name);
            hasCollided = true;
            if (isIt)
            {
                isIt = false;
                collision.gameObject.GetComponent<NetworkCollisions>().isIt = true;
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
