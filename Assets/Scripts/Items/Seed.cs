using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Seed : Item
{
    [SerializeField] private ScoreSO scoreSO;
    private readonly List<Collider2D> _collider2Ds = new List<Collider2D>();
    private SeedsRemainingLabel _seedsRemainingLabel;
    private ScoreCounter _scoreCounter;

    private bool _landCustomHasBeedCalled;

    [SerializeField] private UnityEvent OnSpecialDestroy;

    private void Start()
    {
        _scoreCounter = FindObjectOfType<ScoreCounter>();
        if (_scoreCounter != null) OnSpecialDestroy.AddListener(delegate { _scoreCounter.AddScore(scoreSO); });

        _seedsRemainingLabel = FindObjectOfType<SeedsRemainingLabel>();
        if (_seedsRemainingLabel != null) OnSpecialDestroy.AddListener(_seedsRemainingLabel.OnSpecialDestroyAction);
    }

    protected override void ActivateCustom()
    {
        float randTorque = UnityEngine.Random.Range(-50f, 50f);

        Launch(0f, 800f);
        rb.AddTorque(randTorque);
    }

    public override void SpawnLaunch()
    {
        if (transform.position.x <= 0)
        {
            Launch(300, 600, 150, 300);
        }
        else
        {
            Launch(-300, -600, 150, 300);
        }
    }

    protected override void BounceCustom()
    {
        Launch(0, 400);
    }

    protected override void LandCustom()
    {
        if (_landCustomHasBeedCalled) return;
        _landCustomHasBeedCalled = true;

        c2D.GetContacts(GameState.GrassBlockFilter, _collider2Ds);

        if (_collider2Ds.Count == 0) 
        { 
            IsLanded = false;
            return;
        }

        if (_collider2Ds[0].GetComponent<GrassBlock>() != null)
        {
            if (_collider2Ds[0].GetComponent<GrassBlock>().GetIsFertile())
            {
                _collider2Ds[0].GetComponent<GrassBlock>().PlantSeed();
                Destroy(gameObject);
            }
            else
            {
                var transform1 = transform;
                var position = transform1.position;

                transform1.position = new Vector3(position.x, position.y, 2f);
                spriteRenderer.color = new Color(1f, 1f, 1f, 0.5f);

                OnSpecialDestroy.Invoke();

                Destroy(gameObject, 0.5f);
            }
        }
    }

    protected override void RiseCustom()
    {
    }

    protected override void FallCustom()
    {
        IsActivated = false;
    }

    protected override void DestroyCustom()
    {
        Destroy(gameObject);
    }

    public void GetEaten()
    {
        OnSpecialDestroy.Invoke();
        Destroy(gameObject);
    }
}