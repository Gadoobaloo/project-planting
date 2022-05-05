using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Seed : Item
{
    private readonly List<Collider2D> _collider2Ds = new List<Collider2D>();

    protected override void ActivateCustom()
    {
        float randTorque = Random.Range(-50f, 50f);

        Launch(0f, 800f);
        rb.AddTorque(randTorque);
    }

    public override void SpawnLaunch()
    {
        GameState.SetNumOfSeeds(GameState.GetNumOfSeeds() +1);
        
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

    public override void BounceCustom()
    {
        Launch(-50f, 50f, 400f, 400f);
    }

    protected override void LandCustom()
    {
        c2D.GetContacts(FilterGrassBlock, _collider2Ds);
        
        if (_collider2Ds.Count == 0) SetIsLanded(false);

        foreach (var t in _collider2Ds.Where(t => t.GetComponent<GrassBlock>() != null))
        {
            if (t.GetComponent<GrassBlock>().GetIsFertile())
            {
                t.GetComponent<GrassBlock>().PlantSeed();
                Destroy(gameObject);
            }
            else
            {
                var transform1 = transform;
                var position = transform1.position;
                transform1.position = new Vector3(position.x, position.y, 2f);
                spriteRenderer.color = new Color(1f, 1f, 1f, 0.5f);
                Destroy(gameObject, 0.5f);
            }
        }
    }

    protected override void RiseCustom()
    {
    }

    protected override void FallCustom()
    {
        SetIsActivated(false);
    }

    protected override void DestroyCustom()
    {
        GameState.SetNumOfSeeds(GameState.GetNumOfSeeds() -1);
        Destroy(gameObject);
    }
}
