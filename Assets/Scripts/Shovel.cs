using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Shovel : Item
{
    private bool _isActivated;
    private List<Collider2D> collider2Ds = new List<Collider2D>();


    private void Update()
    {
        if (_isActivated && rb.velocity.y < 0)
        {
            Dive();
        }

        if (c2D.IsTouching(FilterGrassBlock) && !IsLanded)
        {
            Land();
        }
    }

    public override void Activate()
    {
        rb.freezeRotation = false;
        rb.AddTorque(-500f);
        rb.AddForce(new Vector2(0, 4000));
        _isActivated = true;
    }

    public override void SpawnLaunch()
    {
        Debug.Log("shovel spawned");
    }

    private void Dive()
    {
        _isActivated = false;

        rb.freezeRotation = true;
        rb.rotation = 0;
        rb.AddForce(new Vector2(0, -10000));
    }

    private void Land()
    {
        IsLanded = true;

        c2D.GetContacts(FilterGrassBlock, collider2Ds);

        if (collider2Ds.Count == 0) IsLanded = false;
        
        foreach (var t in collider2Ds.Where(t => t.GetComponent<GrassBlock>() != null))
        {
            if (t.GetComponent<GrassBlock>().GetIsFertile())
            {
                spriteRenderer.color = new Color(1f, 1f, 1f, 0.5f);
                Destroy(gameObject, 0.5f);            }
            else
            {
                t.GetComponent<GrassBlock>().ShovelItemLand();
                Destroy(gameObject);
            }
        }
    }
}
