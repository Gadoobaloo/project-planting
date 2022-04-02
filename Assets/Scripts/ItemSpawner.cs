using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private Transform seedPrefab;
    private int _cooldown;

    void Start()
    {
        _cooldown = 10;
    }

    void Update()
    {
        _cooldown--;

        if (_cooldown <= 0)
        {
            SpawnSeed();
        }
    }

    private void SpawnSeed()
    {
        Vector3 pos = transform.position;
        
        Instantiate(seedPrefab, pos, Quaternion.identity);
        _cooldown = 60;
    }
}
