using System.Collections.Generic;
using UnityEngine;

//sunflower is its own prefab that gets instantiated as a child of grass block;

public class Sunflower : MonoBehaviour
{
    [SerializeField] private Transform stemPrefab;
    [SerializeField] private Transform headPrefab;

    private float _timer;

    private int _minSize = 1;
    private int _maxSize = 6;
    private int _actualSize;
    private int _currentSize;

    private void Start()
    {
        DetermineSize();
        
        _timer = 0;
    }

    private void Update()
    {
        _timer += Time.deltaTime;

        if (_timer >= 1f)
        {
            _timer = 0;
        }
    }

    private void DetermineSize()
    {
        _actualSize = Random.Range(_minSize, _maxSize);
    }
    
    private void GrowStem()
    {
        Instantiate(stemPrefab, transform);
    }

    private void GrowHead()
    {
        Instantiate(headPrefab, transform);
    }

    private void GrowNext()
    {
        if (_currentSize < _maxSize) GrowStem();
    }

    public void SegmentFinish()
    {
        GrowNext();
    }
}