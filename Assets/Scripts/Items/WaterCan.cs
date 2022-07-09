using UnityEngine;

public class WaterCan : Item
{
    [SerializeField] private Transform waterDropletTransform;

    protected override void ActivateCustom()
    {
        c2D.isTrigger = true;
        rb.freezeRotation = false;
        rb.AddTorque(-1000f);
        rb.AddForce(new Vector2(0, 4000));
    }

    protected override void RiseCustom()
    {
    }

    protected override void FallCustom()
    {
        if (IsActivated) Burst();
    }

    protected override void LandCustom()
    {
        spriteRenderer.color = new Color(1f, 1f, 1f, 0.5f);
        Destroy(gameObject, 0.5f);
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
        Launch(0, 4000);
    }

    protected override void DestroyCustom()
    {
    }

    private void Burst()
    {
        IsActivated = false;

        var position = transform.position;

        for (int i = 1; i <= 7; i++)
        {
            var droplet = Instantiate(waterDropletTransform);
            droplet.GetComponent<WaterDroplet>().SpraySpawn(position, i);
        }

        Destroy(gameObject);
    }
}