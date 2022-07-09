using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GrassBlockCollection : MonoBehaviour
{
    private List<GrassBlock> _grassBlocks = new List<GrassBlock>();

    private bool _hasEventBeenCalled = false;

    [SerializeField] private UnityEvent OnAllFlowersGrown;

    public int NumOfFertileGrassBlocks
    {
        get
        {
            int toReturn = 0;
            foreach (GrassBlock block in _grassBlocks)
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
            foreach (GrassBlock block in _grassBlocks)
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
        _grassBlocks = new List<GrassBlock>(GetComponentsInChildren<GrassBlock>());
    }

    private void Update()
    {
        if (CheckIfAllBlocksHaveGrownFlowers() && !_hasEventBeenCalled)
        {
            OnAllFlowersGrown.Invoke();
            _hasEventBeenCalled = true;
        }
    }

    private bool CheckIfAllBlocksHaveGrownFlowers()
    {
        foreach (var block in _grassBlocks)
        {
            if (block.HasGrownFlower == false) return false;
        }

        return true;
    }
}