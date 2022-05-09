using System.Collections.Generic;
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
        if (transform.position.x <= 0)
        {
            Launch(200, 600, 100, 400);
        }
        else
        {
            Launch(-300, -600, 150, 250);
        }
    }

    protected override void BounceCustom()
    {
        Launch(-50f, 50f, 400f, 400f);
    }

    protected override void LandCustom()
    {
        c2D.GetContacts(GameState.GrassBlockFilter, _collider2Ds);

        if (_collider2Ds.Count == 0) IsLanded = false;

        if (_collider2Ds[0].GetComponent<GrassBlock>() != null)
        {
            if (_collider2Ds[0].GetComponent<GrassBlock>().GetIsFertile())
            {
                _collider2Ds[0].GetComponent<GrassBlock>().PlantSeed();
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
        IsActivated = false;
    }

    protected override void DestroyCustom()
    {
        Destroy(gameObject);
    }
}