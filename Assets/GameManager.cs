using RVP;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool roadRules;

    public GameObject player;
    private VehicleParent vp;
    private bool indicatorL;
    private bool indicatorR;

    public int playerScore = 1000;

    private void Start()
    {
        vp = player.GetComponent<VehicleParent>();
    }

    void Update()
    {
        //Debug.Log(playerScore);

        if (vp.playerSpeed > 80)
        {
            LowerScore(3);
        }

    }

    public void LowerScore(int minus)
    {
        if (roadRules)
        {
            playerScore -= minus;
        }

        if (playerScore < 0)
        {
            SceneManager.LoadScene("MenuScene");

        }
    }

    // Update is called once per frame

}
