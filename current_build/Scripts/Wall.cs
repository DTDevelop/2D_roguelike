using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{

    // sprite displayed when player has hit wall
    // so player can see if they've successfully attacked the wall
    public Sprite dmgSprite;
    public int hp = 4;


    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer> ();
    }

    public void DamageWall (int loss)
    {
        // visual feedback on successful wall attack
        spriteRenderer.sprite = dmgSprite;
        hp -= loss;
        if (hp <= 0)
            gameObject.SetActive(false); // disables game object

    }

}
