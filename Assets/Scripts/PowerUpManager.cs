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

    // Change these values to change random interval the powerup will spawn
    private const int MAX_TIMER = 20;
    private const int MIN_TIMER = 5;


    // Start is called before the first frame update
    void Start()
    {
        spawn_time = Random.Range(MIN_TIMER, MAX_TIMER);
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

        if(myRenderer.enabled)  // if enabled, rotate icon 
        {
            icons[powerup - 1].transform.Rotate(Vector3.up * 80 * Time.deltaTime, Space.Self);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")   // set tag of player obj to "Player"
        {

            myCollider.enabled = false;
            myRenderer.enabled = false;
            icons[powerup - 1].GetComponent<Renderer>().enabled = false;

            spawn_time = Random.Range(MIN_TIMER, MAX_TIMER);    // reset timer

            // Access UI of gameobj and use add_powerup() to update UI
            if (powerup == 1)   // store tp in UI, get UI obj attached to specific game obj
            {
                collision.gameObject.transform.Find("UI").gameObject.GetComponent<DisplayRoleScript>().add_tp();
            }
            else // Turbo, Attract/Repulse are instant use
            {
                collision.gameObject.GetComponent<PlayerMove>().add_powerup(powerup);   // getComponenet<name of movement script>
            }
        }
    }

}
