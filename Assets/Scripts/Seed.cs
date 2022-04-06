using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Seed : Item
{
    private List<Collider2D> collider2Ds = new List<Collider2D>();

    public override void Activate()
    {
        Launch(0f, 800f);
    }

    public override void SpawnLaunch()
    {
        if (transform.position.x <= 0)
        {
            //seed is on left
            Launch(200, 600, 100, 400);
        }
        else
        {
            //seed is on right
            Launch(-300, -600, 150, 250);
        }
    }

    private void Update()
    {
        if(transform.position.y < -8) Destroy(gameObject);

        if (c2D.IsTouching(FilterGrassBlock) && !IsLanded)
        {
            Land();
        }
    }

    private void Land()
    {
        IsLanded = true;
        
        c2D.GetContacts(FilterGrassBlock, collider2Ds);
        
        if (collider2Ds.Count == 0)IsLanded = false;

        foreach (var t in collider2Ds.Where(t => t.GetComponent<GrassBlock>() != null))
        {
            if (t.GetComponent<GrassBlock>().GetIsFertile())
            {
                t.GetComponent<GrassBlock>().PlantSeed();
                Destroy(gameObject);
            }
            else
            {
                spriteRenderer.color = new Color(1f, 1f, 1f, 0.5f);
                Destroy(gameObject, 0.5f);
            }
        }
    }
}
