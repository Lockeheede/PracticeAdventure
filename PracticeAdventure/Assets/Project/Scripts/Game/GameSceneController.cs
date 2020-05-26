using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSceneController : MonoBehaviour
{
    [Header("Game")]
    public Player player;

    [Header("UI")]
    public GameObject[] hearts;
    public Text bombText;
    public Text arrowText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            for(int i = 0; i < hearts.Length; i++)
            {
                hearts[i].SetActive(i < player.health);
            }
            //the player.health is being called from the Player class
            //player is an instance of Player
            bombText.text = "Bombs: " + player.bombAmount;
            arrowText.text = "Arrows: " + player.arrowAmount;
        }
        else
        {
            for (int i = 0; i < hearts.Length; i++)
            {
                hearts[i].SetActive(false);
            }
        }
    }
}
