using System.Collections.Generic;
using UnityEngine;

public class Shovel : Item
{
    private List<Collider2D> collider2Ds = new List<Collider2D>();

    public override void ActivateCustom()
    {
        c2D.isTrigger = true;
        rb.freezeRotation = false;
        rb.AddTorque(-1000f);
        rb.AddForce(new Vector2(0, 4000));
    }

    public override void FallCustom()
    {
        if(GetIsActivated()) Dive();
    }

    public override void SpawnLaunch()
    {
    }

    public override void Bounce()
    {
    }

    private void Dive()
    {
        SetIsActivated(false);
        c2D.isTrigger = false;
        rb.freezeRotation = true;
        rb.rotation = 0;
        rb.AddForce(new Vector2(0, -10000));
    }

    public override void LandCustom()
    {
        c2D.GetContacts(FilterGrassBlock, collider2Ds);

        if (collider2Ds.Count == 0) SetIsLanded(false);

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

    public override void RiseCustom()
    {
    }
}
