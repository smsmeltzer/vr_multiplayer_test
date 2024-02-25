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

    public Image heart1;
    public Image heart2;
    public Image heart3;
    private int num_lives;

    public Image turboImage;
    public Image attractImage;
    public Image repulseImage;
    public Image emptyImage;

    [SerializeField] public List<IconAttribute> myIcons;

    // Stores type of powerup player has:
    // -1 = no powerup stored
    // 1 = tp (has seperate counter b/c player can store multiple)
    // 2 = turbo speed
    // 3 = attack/repulse
    private int powerup;

    private float targetTime = 20;
    private bool is_tagger;

    [SerializeField] private TextMeshProUGUI tpText;
    public Image tpImage;
    private int num_tps;

    // Start is called before the first frame update
    void Start()
    {
        roleText.text = "Tagger";
        is_tagger = true;
        timerText.text = "";

        num_lives = 3;
        heart1.enabled = true;
        heart2.enabled = true;
        heart3.enabled = true;

        num_tps = 0;
        tpText.text = num_tps.ToString();
        tpImage.enabled = true;

        powerup = -1;
        emptyImage.enabled = true;
        repulseImage.enabled = false;
        turboImage.enabled = false;
        attractImage.enabled = false;

        gameOverText.enabled = false;

        timerText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if (is_tagger)  
        {
            targetTime -= Time.deltaTime;
            float minutes = Mathf.FloorToInt(targetTime / 60);
            float seconds = Mathf.FloorToInt(targetTime % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

            if (targetTime <= 0.0f)
            {
                lose_life();
                targetTime = 20;
            }
        }
    }

    public void change_role(string role)
    {
        roleText.text = role;
        if (role == "Tagger")
        {
            is_tagger = true;
            targetTime = 20;
        }
    }

    public void lose_life()
    {
        num_lives--;
        if (num_lives == 0)
        {
            heart3.enabled = false;
            gameOverText.enabled = true;
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

    public void add_powerup(int p)
    {
        // player has a powerup stored, can't pick up another
        if (powerup != -1)
        {
            return;
        }

        powerup = p;

        // change text based off of type of power up
        if (powerup == 1)
        {
            add_tp();
        }
        else if (powerup == 2)
        {
            emptyImage.enabled = false;
            turboImage.enabled = true;
        }
        else if (powerup == 3)
        {
            emptyImage.enabled = false;

            if (is_tagger)
            {
                attractImage.enabled = true;
            }
            else { 
                repulseImage.enabled = true;
            }
        }
    }

    public void use_powerup()
    {
        if (powerup != -1)
        {
            powerup = -1;
            emptyImage.enabled = true;
            repulseImage.enabled = false;
            turboImage.enabled = false;
            attractImage.enabled = false;
        }
    }
}
