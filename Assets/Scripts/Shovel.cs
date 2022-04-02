using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shovel : Item
{
    private bool _isActivated;

    private void Update()
    {
        if (_isActivated && rb.velocity.y < 0)
        {
            Dive();
        }
        
        
        //if the shovel lands on a grassblock
        //check if the grass block is fertile
        //if it is, despawn the shovel
        //if it isn't,  
    }

    public override void Activate()
    {
        rb.freezeRotation = false;
        rb.AddTorque(-500f);
        rb.AddForce(new Vector2(0, 4000));
        _isActivated = true;
    }

    private void Dive()
    {
        _isActivated = false;

        rb.freezeRotation = true;
        rb.rotation = 0;
        rb.AddForce(new Vector2(0, -10000));
    }
}
