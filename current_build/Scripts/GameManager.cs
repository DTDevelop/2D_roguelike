using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// setup gamemanager to test boardmanager previously written
public class GameManager : MonoBehaviour
{
    // public -- accessible outside of class
    // static -- variable belongs to class itself, opposed to instance of class
    // can access public functions/variables of gamemanager from any script in game
    public static GameManager instance = null;
    // create public variable of boardmanager called boardscript
    public BoardManager boardScript;
    public int playerFoodPoints = 100;
    [HideInInspector] public bool playersTurn = true;
    // hide, although public, won't be displayed in editor

    private int level = 3;
    // testing level 3 as this is where enemies begin to appear


    // start is called before the first frame update
    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
            // avoids having 2 instance of game manager

        DontDestroyOnLoad(gameObject);
        // when loading new scene, normally all objects in hierarchy destroyed
        // used to keep track of score between scenes, allows to persist between scenes
      // get&store component reference to board manager script
        boardScript = GetComponent<BoardManager>();
      // call init game function
        InitGame();

    }

    // declare init game
    void InitGame()
    {
      // call setup scene function of boardscript
      // pass in parameter level, scene takes level specified
      // can determine num of enemies
        boardScript.SetupScene(level);
    }

    public void GameOver()
    {
        // disables gameManager
        enabled = false;

    }

    // Update is called once per frame
    void Update()
    {

    }
}
