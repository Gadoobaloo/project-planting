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
}
