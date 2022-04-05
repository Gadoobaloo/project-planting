using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private Transform seedPrefab;
    [SerializeField] private Transform shovelPrefab;
    private int _cooldown;

    private void Start()
    {
        _cooldown = 10;
    }

    private void Update()
    {
        _cooldown--;
        
        //have the chance of a particular item spawning be random

        if (_cooldown <= 0)
        {
            SpawnSeed();
        }
    }

    private void SpawnSeed()
    {
        Instantiate(seedPrefab, transform.position, Quaternion.identity);
        _cooldown = 60;
    }

    private void SpawnShovel()
    {
        Instantiate(shovelPrefab, transform.position, Quaternion.identity);
    }
}
