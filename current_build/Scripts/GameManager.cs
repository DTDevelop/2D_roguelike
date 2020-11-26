using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// setup gamemanager to test boardmanager previously written
public class GameManager : MonoBehaviour
{
    // create public variable of boardmanager called boardscript
    public BoardManager boardScript;

    private int level = 3;
    // testing level 3 as this is where enemies begin to appear


    // start is called before the first frame update
    void Awake()
    {
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

    // Update is called once per frame
    void Update()
    {

    }
}
