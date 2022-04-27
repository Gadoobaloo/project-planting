using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WaterDroplet : Item
{
    private List<Collider2D> _collider2Ds = new List<Collider2D>();
    
    public override void ActivateCustom()
    {
    }

    public override void RiseCustom()
    {
    }

    public override void FallCustom()
    {
    }

    public override void LandCustom()
    {
    }

    public override void SpawnLaunch()
    {
    }

    public override void Bounce()
    {
    }

    public void SpraySpawn(Vector3 pos ,int i)
    {
        transform.position = pos;

        const float height = 100f;
        
        switch (i)
        {
            case 1:
                Launch(-150, height);
                break;
            case 2:
                Launch(-100, height);
                break;
            case 3:
                Launch(-50, height);
                break;
            case 4:
                Launch(0, height);
                break;
            case 5:
                Launch(50, height);
                break;
            case 6:
                Launch(100, height);
                break;
            case 7:
                Launch(150, height);
                break;
            default:
                Debug.LogError("SpraySpawn parameter " + i + " not accounted for");
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.GetComponent<GrassBlock>() != null)
        {
            col.GetComponent<GrassBlock>().SpeedUpFlowerGrowth();
        }
    }
}
