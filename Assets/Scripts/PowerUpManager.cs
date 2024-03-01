using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon.StructWrapping;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.Rendering;

public class NewBehaviourScript : MonoBehaviour
{
    private Collider myCollider;
    private Renderer myRenderer;
    private float spawn_time;

    // 1 = add tp
    // 2 = add turbo speed
    // 3 = add attract/repulse  
    private int powerup;

    public List<GameObject> icons;


    // Start is called before the first frame update
    void Start()
    {
        spawn_time = Random.Range(1, 15);
        myCollider = this.GetComponent<Collider>();
        myCollider.enabled = false; 
        myRenderer = this.GetComponent<Renderer>();
        myRenderer.enabled = false;

        for(int i = 0; i < icons.Count; i++)
        {
            icons[i].GetComponent<SpriteRenderer>().enabled = false;
        }

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
                icons[powerup - 1].GetComponent<Renderer>().enabled = true;  
            }
        }

        if(myRenderer.enabled)
        {
            icons[powerup - 1].transform.Rotate(Vector3.up * 80 * Time.deltaTime, Space.Self);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Powerup collided with " + collision.gameObject.tag);
        if (collision.gameObject.tag == "Player")   // set tag of player obj to "Player"
        {

            myCollider.enabled = false;
            myRenderer.enabled = false;
            icons[powerup - 1].GetComponent<Renderer>().enabled = false;

            spawn_time = Random.Range(15, 30);    // reset timer

            // Access UI of gameobj and use add_powerup() to update UI
            collision.gameObject.transform.GetChild(0).gameObject.GetComponent<DisplayRoleScript>().add_powerup(powerup);
        }
    }

}
