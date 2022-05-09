using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private Transform seedPrefab;
    [SerializeField] private Transform shovelPrefab;
    [SerializeField] private Transform waterCanPrefab;

    [SerializeField] private GrassBlockCollection grassBlockCollection;

    private float _timer = 1f;

    private void Update()
    {
        _timer -= Time.deltaTime;

        if (_timer <= 0f)
        {
            _timer = Random.Range(2f, 7f);

            SpawnItem(DetermineItem());

            if (CoinFlip())
            {
                SwapSides();
            }
        }
    }

    private void SpawnItem(Transform toSpawn)
    {
        var item = Instantiate(toSpawn, transform.position, Quaternion.identity);
        item.GetComponent<Item>().SpawnLaunch();
    }

    private bool CoinFlip()
    {
        return Random.Range(0, 2) == 0;
    }

    private void SwapSides()
    {
        transform.position = new Vector3(-transform.position.x, transform.position.y, 11);
    }

    private Transform DetermineItem()
    {
        List<Transform> items = new List<Transform>();

        //SEED
        int seedWieght = 0;

        if (GameState.NumOfSeeds < 3)
        {
            seedWieght = Random.Range(7, 15);
        }
        else
        {
            seedWieght = Random.Range(2, 4);
        }

        while (seedWieght > 0)
        {
            items.Add(seedPrefab);
            seedWieght--;
        }

        //SHOVEL
        int shovelWieght = 0;

        int tempMax = 3 - grassBlockCollection.NumOfFertileGrassBlocks;
        if (tempMax < 0) tempMax = 0;

        shovelWieght = Random.Range(0, tempMax);

        while (shovelWieght > 0)
        {
            items.Add(shovelPrefab);
            shovelWieght--;
        }

        //WATER CAN
        int waterCanWieght = Random.Range(1, grassBlockCollection.NumOfFlowersGrowing + 2);

        if (waterCanWieght > 0)
        {
            items.Add(waterCanPrefab);
            waterCanWieght--;
        }

        return items[Random.Range(0, items.Count)];
    }
}