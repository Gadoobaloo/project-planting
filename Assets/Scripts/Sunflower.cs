using UnityEngine;

public class Sunflower : MonoBehaviour
{
    [SerializeField] private Transform stemPrefab;
    [SerializeField] private Transform headPrefab;

    private const int MaxSize = 8;
    private int _goalSize;

    private void Start()
    {
        transform.localPosition = new Vector3(0, 1, 0);
        DetermineSize();
        GrowNext();
    }
    
    private void DetermineSize()
    {
        _goalSize = Random.Range(2, MaxSize);
    }
    
    private void GrowStem()
    {
        Instantiate(stemPrefab, transform);

        var childCount = transform.childCount;
        transform.GetChild(childCount -1).GetComponent<SunflowerPart>().SetHeight(childCount -1);
    }

    private void GrowHead()
    {
        Instantiate(headPrefab, transform);
        
        var childCount = transform.childCount;
        transform.GetChild(childCount -1).GetComponent<SunflowerPart>().SetHeight(childCount -1);
    }

    private void GrowNext()
    {
        var childCount = transform.childCount;

        if (childCount < _goalSize - 1) GrowStem();
        if (childCount == _goalSize - 1) GrowHead();
        if (childCount >= _goalSize) Debug.Log("sunflower has finished growing");
    }

    public void SegmentFinish()
    {
        GrowNext();
    }
}