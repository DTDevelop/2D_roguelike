using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MovingObject
{

    public int wallDamage = 1;
    public int pointsPerFood = 10;
    public int pointsPerSoda = 20; // num added to item per pickup
    public float restartLevelDelay = 1f;

    private Animator animator; // used to store reference to animator component

    // stores player score during level before passing back to game manager as we change levels
    private int food;

    // Start is called before the first frame update

    // add protected and override, have different start in player class vs movingobject class
    protected override void Start()
    {
        animator = GetComponent<Animator> ();

        // enables player to manage food score during level
        // then store in game manager as we change levels
        food = GameManager.instance.playerFoodPoints;

        base.Start();
    }

    // OnDisable a part of unity API
    // called when player game object is disabled
    // used to store vallue of food in game manager as we change levels
    private void OnDisable()
    {
        GameManager.instance.playerFoodPoints = food;

    }

    // Update is called once per frame
    void Update()
    {
        // check if players turn using boolean in game manager
        if (!GameManager.instance.playersTurn) return;
        // code that follows will not be executed if not

        int horizontal = 0;
        int vertical = 0; // store direction moving, either as 1 or -1, along axis

        // get input from input manager
        // cast from float to an int
        // store in horizontal/vertical variable declared

        horizontal = (int) Input.GetAxisRaw("Horizontal");
        vertical = (int) Input.GetAxisRaw("Vertical");

        // current movement code only will enable keyboard / controller input
        // for stnadalone build of game

        // later, version of movement code will take mobile / touch screen input

        if (horizontal != 0)
            vertical = 0; // prevent player from diagonal movements

        if (horizontal != 0 || vertical != 0)
            AttemptMove<Wall> (horizontal, vertical);
            // passing generic param. "wall", player may encounter wall (interactable object)
            // previously, generic param. T, allows us to specify what we interact with when we call the function
    }

    protected override void AttemptMove <T> (int xDir, int yDir)
    // takes generic parameter "T", specify type of component expecting our mover to encounter
    {
        food--;

        base.AttemptMove <T> (xDir, yDir);

        RaycastHit2D hit; // reference result of line cast done in move

        CheckIfGameOver(); // since player has just lost foodpoints upon movement

        GameManager.instance.playersTurn = false;
    }

    // want player to interact with exit, soda, food objects
    // use onTriggerEnter2D, part of the unity API
    private void OnTriggerEnter2D (Collider2D other)
    {
        // previously set exit, soda, food prefab colliders to IsTrigger
        // check tag of other object colliding with

        if (other.tag == "Exit")
        {
            Invoke ("Restart", restartLevelDelay);
            enabled = false;
        }
        else if (other.tag == "Food")
        {
            food += pointsPerFood;
            other.gameObject.SetActive(false);
        }
        else if (other.tag == "Soda")
        {
            food += pointsPerSoda;
            other.gameObject.SetActive(false);
        }

    }

    protected override void OnCantMove <T> (T component)
    {
        // previously declared as abstract without any implementation
        // protected override function returning void
        // taking generic param. "T", as well as param. type "T" called component

        // want player to take action if trying to move into space with wall and are blocked by it
        Wall hitWall = component as Wall;
        // type Wall == component passed in as paramter casting it to a Wall

        hitWall.DamageWall(wallDamage); // call DamageWall of the wall hit
        animator.SetTrigger("playerAttack");

    }

    private void Restart()
    {
        // if player collides with exit object
        // go to next level

        Application.LoadLevel(Application.loadedLevel);
        // loading last scene that was loaded, "main", only scene in the game
        // many games, to load another level we load another scene
        // in this case, loading same scene as levels created procedurally via script

    }

    public void LoseFood (int loss)
    {
        // calls when enemy attacks player
        // loss specifies how many points player will lose
        animator.SetTrigger("playerHit");
        food = loss;
        CheckIfGameOver();

    }

    private void CheckIfGameOver()
    {
        if (food <= 0)
            GameManager.instance.GameOver();

    }

}





















//
