using System.Collections.Generic;
using UnityEngine;

public class Shovel : Item
{
    private List<Collider2D> collider2Ds = new List<Collider2D>();

    protected override void ActivateCustom()
    {
        c2D.isTrigger = true;
        rb.freezeRotation = false;
        rb.AddTorque(-1000f);
        rb.AddForce(new Vector2(0, 4000));
    }

    protected override void FallCustom()
    {
        if (IsActivated) Dive();
    }

    public override void SpawnLaunch()
    {
        if (transform.position.x <= 0)
        {
            Launch(1000, 3000, 4000, 5000);
        }
        else
        {
            Launch(-1000, -3000, 4000, 5000);
        }
    }

    protected override void BounceCustom()
    {
    }

    protected override void DestroyCustom()
    {
    }

    private void Dive()
    {
        IsActivated = false;
        c2D.isTrigger = false;
        rb.freezeRotation = true;
        rb.rotation = 0;
        rb.AddForce(new Vector2(0, -10000));
    }

    protected override void LandCustom()
    {
        c2D.GetContacts(GameState.GrassBlockFilter, collider2Ds);

        if (collider2Ds.Count == 0) IsLanded = false;

        if (collider2Ds[0].GetComponent<GrassBlock>() != null)
        {
            if (collider2Ds[0].GetComponent<GrassBlock>().GetIsFertile())
            {
                spriteRenderer.color = new Color(1f, 1f, 1f, 0.5f);
                Destroy(gameObject, 0.5f);
            }
            else
            {
                collider2Ds[0].GetComponent<GrassBlock>().ShovelItemLand();
                Destroy(gameObject);
            }
        }
    }

    protected override void RiseCustom()
    {
    }
}