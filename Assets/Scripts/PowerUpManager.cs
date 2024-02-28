using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon.StructWrapping;
using Unity.XR.CoreUtils;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    private Collider myCollider;
    private Renderer myRenderer;
    private float spawn_time;
    private int powerup;


    // Start is called before the first frame update
    void Start()
    {
        spawn_time = Random.Range(1, 15);
        myCollider = this.GetComponent<Collider>();
        myCollider.enabled = false; 
        myRenderer = this.GetComponent<Renderer>();
        myRenderer.enabled = false;

        powerup = -1;
    }

    // Update is called once per frame
    void Update()
    {
        // if not enabled, continue counting down until enabled and spawn powerup
        if (!myRenderer.enabled) { 
            spawn_time -= Time.deltaTime;
            if (spawn_time <= 0)
            {
                myCollider.enabled = true;
                myRenderer.enabled = true;

                powerup = Random.Range(1, 4);   // choose random powerup to spawn
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("Collided with " + collision.gameObject.name);

        myCollider.enabled = false;
        myRenderer.enabled= false;

        spawn_time = Random.Range(15, 30);    // reset timer

        // Access UI of gameobj and use add_powerup() to update UI
        collision.gameObject.transform.GetChild(0).gameObject.GetComponent<DisplayRoleScript>().add_powerup(powerup);
    }

}
