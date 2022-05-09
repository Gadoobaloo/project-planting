using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Sunflower : MonoBehaviour
{
    [SerializeField] private Transform stemPrefab;
    [SerializeField] private Transform headPrefab;

    private const int MaxSize = 8;
    private const int MinSize = 3;
    private int _goalSize;

    private bool _isWatered;
    private bool _isInfested;

    public bool IsInfested
    { get { return _isInfested; } }

    public bool IsGrowing { get; private set; }

    public static event EventHandler OnSunflowerGrown;

    private void Start()
    {
        IsGrowing = true;
        transform.localPosition = new Vector3(0, 1, 0);
        DetermineSize();
        GrowNext();
    }

    private void DetermineSize()
    {
        _goalSize = Random.Range(MinSize, MaxSize);
    }

    private void GrowStem()
    {
        var part = Instantiate(stemPrefab, transform);

        part.GetComponent<SunflowerPart>().ParentInitialize(transform.childCount - 1, _isWatered, _isInfested);
    }

    private void GrowHead()
    {
        var part = Instantiate(headPrefab, transform);

        part.GetComponent<SunflowerPart>().ParentInitialize(transform.childCount - 1, _isWatered, _isInfested);
    }

    private void GrowNext()
    {
        var childCount = transform.childCount;

        if (childCount < _goalSize - 1) GrowStem();
        if (childCount == _goalSize - 1) GrowHead();
        if (childCount >= _goalSize)
        {
            OnSunflowerGrown?.Invoke(this, EventArgs.Empty);
            IsGrowing = false;
        }
    }

    public void SegmentFinish()
    {
        GrowNext();
    }

    public void Water()
    {
        if (!_isWatered) _isWatered = true;
        transform.GetChild(transform.childCount - 1).GetComponent<SunflowerPart>().SetIsWatered(true);
    }
}