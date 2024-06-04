using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Rules()
    {
        SceneManager.LoadScene("GameScene");

    }
    public void Speedy()
    {
        SceneManager.LoadScene("GameScene 1");
    }
}
