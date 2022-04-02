using UnityEngine;

public class Seed : Item
{
    public override void Activate()
    {
        Launch(0f, 800f);
    }
}
