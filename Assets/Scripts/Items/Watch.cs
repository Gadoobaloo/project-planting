using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Watch : Item
{
    private List<Collider2D> collider2Ds = new List<Collider2D>();

    public override void SpawnLaunch()
    {
        if (transform.position.x <= 0)
        {
            Launch(300, 600, 150, 300);
        }
        else
        {
            Launch(-300, -600, 150, 300);
        }
    }

    protected override void ActivateCustom()
    {
        TeleportHide();
        StartCoroutine(SlowTime());
    }

    protected override void BounceCustom()
    {
        Launch(0, 400);
    }

    protected override void DestroyCustom()
    {
        Destroy(gameObject);
    }

    protected override void FallCustom()
    {
    }

    protected override void LandCustom()
    {
        c2D.GetContacts(GameState.GrassBlockFilter, collider2Ds);

        if (collider2Ds.Count == 0) IsLanded = false;

        if (collider2Ds[0].GetComponent<GrassBlock>() != null)
        {
            spriteRenderer.color = new Color(1f, 1f, 1f, 0.5f);
            Destroy(gameObject, 0.5f);
        }
    }

    protected override void RiseCustom()
    {
    }

    private IEnumerator SlowTime()
    {
        Time.timeScale = 0.5f;
        yield return new WaitForSeconds(7);
        Time.timeScale = 1f;
        DestroyUniversal();
    }

    private void TeleportHide()
    {
        transform.position = new Vector3(-100, 0, 0);
        rb.bodyType = RigidbodyType2D.Static;
    }
}