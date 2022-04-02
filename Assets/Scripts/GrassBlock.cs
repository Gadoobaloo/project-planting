using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassBlock : MonoBehaviour
{
    [SerializeField]private SpriteRenderer spriteRendererShovel;

    void Start()
    {
        HideShovel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HideShovel()
    {
        spriteRendererShovel.color = new Color(1f, 1f, 1f, 0f);
    }

    public void ShowShovel()
    {
        spriteRendererShovel.color = new Color(1f, 1f, 1f, 1f);
    }

    void Dig()
    {
        //play shovel animation
        //have progress bar show
        //allow for cancel
        //after successful dig, apply fertilized soil state
    }
}
