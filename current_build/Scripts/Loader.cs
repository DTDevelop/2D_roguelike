using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// check if game has been instantiated
// if not, instantiate one from prefab

public class Loader : MonoBehaviour
{

    public GameObject gameManager;

    void Awake()
    {
        // using public variable created in gamemanager script
        // accessing from this loader script
    if (GameManager.instance == null)
            Instantiate(gameManager);
    }
}
