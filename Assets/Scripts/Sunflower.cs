using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class Sunflower : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform stemPrefab;
    [SerializeField] private Transform headPrefab;
    [SerializeField] private ScoreSO scoreSO;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip pluckSFX;

    private ScoreCounter _scoreCounter;
    private SunflowerCounter _sunflowerCounter;

    private const int MaxSize = 8;
    private const int MinSize = 3;
    private int _goalSize;

    private bool _isMoving = false;
    private float _movementSpeed = 10.0f;

    public bool IsWatered { get; private set; }
    private bool _isInfested;

    public bool IsInfested
    { get { return _isInfested; } }

    public bool IsGrowing { get; private set; }
    public bool HasGrown { get; private set; }

    [SerializeField] private UnityEvent OnSunflowerGrown;

    private void Start()
    {
        _sunflowerCounter = FindObjectOfType<SunflowerCounter>();
        if (_sunflowerCounter != null) OnSunflowerGrown.AddListener(_sunflowerCounter.OnSunflowerGrownAction);

        _scoreCounter = FindObjectOfType<ScoreCounter>();
        if (_scoreCounter != null) OnSunflowerGrown.AddListener(delegate { _scoreCounter.AddScore(scoreSO); });

        OnSunflowerGrown.AddListener(transform.parent.GetComponent<GrassBlock>().HarvestSunflower);

        IsGrowing = true;
        transform.localPosition = new Vector3(0, 1, 0);
        DetermineSize();
        GrowNext();
    }

    private void Update()
    {
        if (_isMoving)
        {
            transform.Translate(Vector3.up * _movementSpeed * Time.deltaTime);
            if (transform.position.y >= -2) _isMoving = false;
        }
    }

    private void DetermineSize()
    {
        _goalSize = Random.Range(MinSize, MaxSize);
    }

    private void GrowStem()
    {
        var part = Instantiate(stemPrefab, transform);

        part.GetComponent<SunflowerPart>().ParentInitialize(transform.childCount - 2, IsWatered, _isInfested);
    }

    private void GrowHead()
    {
        var part = Instantiate(headPrefab, transform);

        part.GetComponent<SunflowerPart>().ParentInitialize(transform.childCount - 2, IsWatered, _isInfested);
    }

    private void GrowNext()
    {
        var childCount = transform.childCount;

        if (childCount < _goalSize - 1) GrowStem();
        if (childCount == _goalSize - 1) GrowHead();
        if (childCount >= _goalSize)
        {
            OnSunflowerGrown.Invoke();
            IsGrowing = false;
            HasGrown = true;
        }
    }

    public void SegmentFinish()
    {
        GrowNext();
    }

    public void Water()
    {
        if (!IsWatered) IsWatered = true;
        transform.GetChild(transform.childCount - 1).GetComponent<SunflowerPart>().SetIsWatered(true);
    }

    /// <summary>
    /// To be called by a parent GrassBlock.
    /// </summary>
    public void Harvest()
    {
        StartCoroutine(HarvestAnimation());
    }

    /// <summary>
    /// Moves the sunflower up, waits for one second, then Destroys the GameObject
    /// </summary>
    /// <returns></returns>
    private IEnumerator HarvestAnimation()
    {
        _isMoving = true;
        audioSource.PlayOneShot(pluckSFX, GameSettings.volumeSFX);

        yield return new WaitForSecondsRealtime(1f);

        Destroy(gameObject);
    }
}