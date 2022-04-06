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
        Instantiate(seedPrefab, transform.position, Quaternion.identity);
    }

    private void SpawnShovel()
    {
        Instantiate(shovelPrefab, transform.position, Quaternion.identity);
    }
}
