using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private Transform seedPrefab;
    [SerializeField] private Transform shovelPrefab;
    private float _timer;

    private void Update()
    {
        _timer += Time.deltaTime;
        
        //todo - have the chance of a particular item spawning be random

        if (_timer > 2f)
        {
            _timer = 0f;
            SpawnSeed();
        }
    }

    private void SpawnSeed()
    {
        var seed = Instantiate(seedPrefab, transform.position, Quaternion.identity);
        seed.GetComponent<Seed>().SpawnLaunch();
    }

    private void SpawnShovel()
    {
        var shovel = Instantiate(shovelPrefab, transform.position, Quaternion.identity);
        shovel.GetComponent<Shovel>().SpawnLaunch();
    }
}
