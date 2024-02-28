using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DisplayRoleScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI roleText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI gameOverText;

    [SerializeField] private Image heart1;
    [SerializeField] private Image heart2;
    [SerializeField] private Image heart3;
    private int num_lives;

    [SerializeField] private Image tpImage;

    const int MAX_TAGGER_TIME = 120;    // max time given to tagger 

    private float targetTime;
    private bool is_tagger;

    [SerializeField] private TextMeshProUGUI tpText;
    private int num_tps;

    // Start is called before the first frame update
    void Start()
    {
        set_role("runner");

        num_lives = 3;
        heart1.enabled = true;
        heart2.enabled = true;
        heart3.enabled = true;

        num_tps = 0;
        tpText.text = num_tps.ToString();

        gameOverText.enabled = false;

        timerText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if (is_tagger)  // must count down tagger's remaining time
        {
            targetTime -= Time.deltaTime;
            float minutes = Mathf.FloorToInt(targetTime / 60);
            float seconds = Mathf.FloorToInt(targetTime % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);  // display time in min:sec format

            if (targetTime <= 0.0f) // when out of time, lose life and reset targetTime
            {
                lose_life();
                targetTime = MAX_TAGGER_TIME;
            }
        }
    }

    // Sets role to given role as a String, will display as inputted
    public void set_role(string role)
    {
        roleText.text = role;
        if (role.ToLower() == "tagger")
        {
            is_tagger = true;
            targetTime = MAX_TAGGER_TIME;
        }
        else
        {
            is_tagger = false;
        }
    }

    // Subtracts a life from player
    // Returns if player is alive after subtracting life
    public bool lose_life()
    {
        if (is_alive())
        {
            num_lives--;
            if (num_lives == 0)
            {
                // Player has died: disable all UI
                heart3.enabled = false; 
                gameOverText.enabled = true;
                timerText.enabled = false;
                roleText.enabled = false;
                tpText.enabled = false;
                tpImage.enabled = false;

            }
            else if (num_lives == 1)
            {
                heart2.enabled = false;
            }
            else
            {
                heart1.enabled = false;
            }
        }
        return is_alive();
    }

    public bool is_alive()
    {
        return num_lives > 0;
    }

    public void add_tp()
    {
        num_tps++;
        tpText.text = num_tps.ToString();
    }

    public void use_tp()
    {
        num_tps--;
        tpText.text = num_tps.ToString();
    }
}
