using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// abstract enalbes to create classes & class members incomplete
// must be implemeneted in derived class
public abstract class MovingObject : MonoBehaviour
{

    public float moveTime = 0.1f; // object to move in seconds
    public LayerMask blockingLayer;


    private BoxCollider2D boxCollider;
    private Rigidbody2D rb2D; // store component reference to rigidbody component of the unit moving
    private float inverseMoveTime; // make movement calculations more efficient

    // protected virtual functions can be overridden
    // by their inheriting classes
    // useful if want inheriting classees to have different implementaiton of start
    protected virtual void Start()
    {
        boxCollider = GetComponent<BoxCollider2D> ();
        rb2D = GetComponent<Rigidbody2D> ();
        inverseMoveTime = 1f / moveTime; // used to multiply, computational efficiency

    }


    protected bool Move (int xDir, int yDir, out RaycastHit2D hit)
    {
        // out, causes arguments to be passed by reference
        // using to return more than one value in omve function
        // have boolean "Move", as well as RaycastHit2D "hit"

        Vector2 start = transform.position;
        // transform.position is vector3, casting as vector2 implicitly converting
        // discards z axis data
        Vector2 end = start + new Vector2 (xDir, yDir);

        boxCollider.enabled = false; // disable, when casting ray, not hitting own collider

        // cast line, from start to end point, checking collision on blocking layer
        hit = Physics2D.Linecast (start, end, blockingLayer);
        boxCollider.enabled = true;

        if (hit.transform == null)
        {
          // if space is open and available to move into
          // starts co routine, smoothMovement, passing param. end

          StartCoroutine(SmoothMovement (end));
          return true; // able to move

        }

        return false; // unsuccessful movement



    }

    // co routine
    protected IEnumerator SmoothMovement (Vector3 end) // specifies where to move to
    {
        // calculated remaining distance
        // based on sqr magnitude of current pos. & end param.
        // sqrmagnitude is computationally cheaper than magnitutde
        float sqrRemainingDistance = (transform.position - end).sqrMagnitude;

        while (sqrRemainingDistance > float.Epsilon) // represents small num, almost 0
        {
            // find position proportionally closer to end, based on moveTime
            Vector3 newPosition = Vector3.MoveTowards (rb2D.position, end, inverseMoveTime * Time.deltaTime);
            // MoveTowards moves a point in a straight line towards a target point
            // 3rd param. represents value moving closer to end pos.
            rb2D.MovePosition(newPosition);
            // calculate remaining distance after moving
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;

            yield return null; // waiting 1 frame before evaluating condition of loop

        }

    }

    protected virtual void AttemptMove <T> (int xDir, int yDir)
        where T : Component
        // generic param. T used to specify type of component
        // we expect our unit to interact with, if blocked
        // case of enemies, it will be player
        // case of player, it will be walls, for player to attack/destroy wall

        // using where, specifying T as Component
    {
        RaycastHit2D hit;
        bool canMove = Move (xDir, yDir, out hit); // T/F if success or not

        // hit being out param, allows us to check transform we hit in move is null
        if (hit.transform == null)
            return;
        // if nothing hit by linecast in move, will return & not execute following code

        // get component reference to the component of type T
        // attached to the object that was hit
        T hitComponent = hit.transform.GetComponent<T>();

        // moving object is blocked & hit something it can interact with
        if (!canMove && hitComponent != null)
            OnCantMove(hitComponent);

    }

    // takes generic param "T"
    // as well as param. type T, copmonent
    protected abstract void OnCantMove <T> (T component)
    // abstract modifier indicates, thing being modified has missing/incomplete implementation
    // OnCantMove willl be overridden by functions in inheriting classes
        where T : Component;
    // has no opening or closing brackets
}



// reason for generic parameter
// haveboth player and enemy inherit from MovingObject
// Player will need to be able to interact with walls
// Enemies will need to be able to interact with player
// do not know what type of hitComponent those 2 will be interacting with
// by making it generic, can get a reference to it
// pass it to OnCantMove, then acts accordingly
// based on implementation of OnCantMove in the inheriting classes



























//
