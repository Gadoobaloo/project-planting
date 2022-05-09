using System.Collections.Generic;
using UnityEngine;

public class GrassBlockCollection : MonoBehaviour
{
    private List<GrassBlock> grassBlocks = new List<GrassBlock>();

    public int NumOfFertileGrassBlocks
    {
        get
        {
            int toReturn = 0;
            foreach (GrassBlock block in grassBlocks)
            {
                if (block.GetIsFertile())
                {
                    toReturn++;
                }
            }
            return toReturn;
        }
    }

    public int NumOfFlowersGrowing
    {
        get
        {
            int toReturn = 0;
            foreach (GrassBlock block in grassBlocks)
            {
                if (block.IsGrowingFlower)
                {
                    toReturn++;
                }
            }

            return toReturn;
        }
    }

    private void Start()
    {
        foreach (GrassBlock grassBlock in transform)
        {
            grassBlocks.Add(grassBlock);
        }
    }
}