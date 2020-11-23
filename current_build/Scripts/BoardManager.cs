using System;
// able to use serializable attribute
// allows us to modify how variables appear in inspector/editor
// show/hide using foldout

using System.Collections;
using System.Collections.Generic;
// allow us to use lists
using UnityEngine;
using Random = UnityEngine.Random;
// specify due to both system & unity namespaces having class random

public class BoardManager : MonoBehaviour
{
    // declare serializable public class, Count
    [Serialiable]
    public class Count
    {
        public int minimum;
        public int maximum;

        // assignment constructor for count
        // able to set new minimum/maximum when declaring new count
        public Count (int min, int max)
        {
            // parameters used to set values of min/max
            minimum = min;
            maximum = max;
        }
    }
    // Declare variables
    // PUBLIC VARIABLES

    // delineate values of gameboard
    public int columns = 8;
    public int rows = 8;

    // using count, specify random range of how many walls for each lvl
    public Count wallCount = new Count (5,9); // min 5, max 9
    public Count foodCount = new Count (1,5); // same for food items

    // declare variables to hold prefabs wanted to spawn that make up gameboard
    public GameObject exit; // only 1 exit object

    // for other objects, place in array, able to pass in multiple objects
    // able to choose 1 want to spawn among the variations
    public GameObject[] floorTiles;
    public GameObject[] wallTiles;
    public GameObject[] foodTiles;
    public GameObject[] enemyTiles; // fill each array with different prefabs
    public GameObject[] outerWallTiles; // to choose between in inspector

    // PRIVATE VARIABLES
    // used to kee hierarchy clean
    // due to so many gameobjects, child all to boardholder
    // able to collapse hierarchy & not fill it with gameobjects
    private Transform boardholder;

    // used to track all different possible positions on gameboard
    // whether an object has been spawned in that position or not
    private List <Vector3> gridPositions = new List<Vector3>();

    void InitialiseList() //returns void
    {
        gridPositions.Clear(); // start by clearing grid positions

        // create nested for loop
        // fill list with each position on gameboard as vector3

        for (int x = 1; x < columns - 1; x++) // x axis
        {
            for (int y = 1; y < rows - 1; y++) // y axis
            {
                // adding the x & y valules to new vector3 to the list gridPositions
                gridPositions.Add(new Vector3(x,y,0f));
                // starting at 1 to -1
                // has border between tiles & outerwalls
                // guarantees levels that are not impassable
            }
        }
    }

    void BoardSetup () // used to setup floor & outerwall of gameboard, returns void
    {
        boardHolder = new GameObject ("Board").transform;
        // use same loop pattern for floor & outerwall tiles
        for (int x = -1; x < columns + 1; x++)
        {
          for (int y = -1; y < rows + 1 y++)
          {
              // starting -1, to +1
              // build edge aroudn active portion of gameboard
              // using outerwall objects

              // choosing floor tiles at random, instantiate it
              GameObject toInstantiate = floorTiles(Random.Range (0, floortiles.Length));
              // declare variable of type Gameobject called toInstantiate
              // setting it to equal an index in an array called floorTiles chosen randomly
              // between 0 and the length of the given array
              // won't have to pre specify length, take the length & choose number within array

              // checking if currently in outer wall position
              // if yes, choose outerwall tile to instantiate
              if (x == -1 || x == columns || y == -1 || y == rows)
              // checks if x is -1, or value for columns
              // checks if y is -1, or value for rows
                toInstantiate = outerWallTiles(Random.Range (0, outerWallTiles.Length));

              // once chosen what tile we want to instantiate, then we instantiate it
              // declare variable called instance, assign it the object we're instantiating

              // call instantiate, pass in toInstantiate (prefab chosen)
              // located at new Vector3 based on current x,y coordinates in the loop passing 0 for z axis as we're in 2d
              // Quaternion idenity == instantiate with no rotation
              GameObject instance = Instantiate(toInstantiate, new Vector 3 (x,y,0f), Quaternion.identity) as GameObject;
              // casting it to a GameObject (explicit conversion)

              // setting parent of our new instantiated gameobject to boardholder
              instance.transform.SetParent(boardHolder);
          }
        }// boardsetup layouts outer wall tiles & background of floor tiles
    }

    // functions that place random objects on gameboard (walls, enemies, powerups)

    Vector3 RandomPosition() // function returns vector3
    {
        int randomIndex = Random.Range (0, gridPositions.Count); // between 0 & num of positions stored in grid position list

        // declare vector3 called randomposition
        // set value as - get value stored at index
        Vector3 randomPosition = gridPositions[randomIndex]
        // make sure 2 objects don't spawn in same location
        // grid position used will be removed from list
        gridPositions.RemoveAt(randomIndex); //using RemoveAt command
        return randomPosition; // used to spawn object in random location
    } // generates random positon from list & ensures it isn't duplicate

    // function that spawns tile at random position
    void LayoutObjectAtRandom(GameObject[] tileArray int minimum, int maximum)
    // takes an array of GameObjects called tileArray, min int, max int
    {
        int objectCount = Random.Range (minimum, maximum +1);
        // controls how many of a given object we're spawning
        // eg. num of walls in level
        for (int 1 = 0; 1  < objectCount; i++)
        {
            // choose random position by calling helper function, randomPosition
            Vector3 randomPosition = RandomPosition();
            // choose random tile from array of game objects tileArray to spawn
            GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];
            // instantiate tile chosen at random position
            instantiate (tileChoice, randomPosition, Quaternion.identity);
        }
    }

    // single public funciton in class, called by gamemanager when time to setup board
    public void SetupScene (int level)
    {
        BoardSetup();
        InitialiseList();
        LayoutObjectAtRandom(wallTiles, wallCount.minimum, wallCount.maximum);
        LayoutObjectAtRandom(foodTiles, foodCount.minimum, foodcount.maxmimum);
        // rather than random amonut of enemies
        // generate num of enemies based on level using Mathf.log, logarithmic difficulty progression
        // Mathf.log returns a float, we cast it as int (explicit conversion)
        int enemyCount = (int)Mathf.Log(level,2f);
        LayoutObjectAtRandom(enemyTiles, enemyCount, enemyCount);// min&max same, not a range

        // exit always placed in same place & object
        Instantiate(exit, new Vector3(columns - 1, rows -1, 0F), Quaternion.identity);
        // upper right corner, so col -1, row -1
        // if gameboard resized, exit placed relative to corner
    }
}
























#
