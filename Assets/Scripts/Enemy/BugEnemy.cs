using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

internal enum MovementType
{ Walk, Fly, Stop }

public class BugEnemy : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private CapsuleCollider2D cc2D;
    [SerializeField] private Animator animator;
    [SerializeField] private ScoreSO scoreSO;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSourceVoice;

    [SerializeField] private AudioSource audioSourceWings;
    [SerializeField] private AudioClip wingsSFX;
    [SerializeField] private AudioClip chewSFX;
    [SerializeField] private AudioClip deathSFX;
    [SerializeField] private AudioClip catchSFX;

    private bool wingSoundIsPlaying;

    private ScoreCounter _scoreCounter;

    private readonly List<Collider2D> _collider2Ds = new List<Collider2D>();

    private bool _isFlying;
    private bool _isEating;
    private bool _isDead;

    private float _eatCooldown = 5f;
    private float _eatSoundCooldown = 1f;

    private MovementType _movementType;
    private float _maxWalkSpeed = 1f;

    private float _maxFlySpeedx = 2f;
    private float _maxFlySpeedy = 1.5f;
    private float _minFlydpeedy = -0.5f;
    private float _flySpeedy;
    private bool _isFacingRight;

    private static readonly int IsEating = Animator.StringToHash("isEating");
    private static readonly int Die1 = Animator.StringToHash("Die");
    private static readonly int Land = Animator.StringToHash("Land");
    private static readonly int Fly1 = Animator.StringToHash("Fly");

    [SerializeField] private UnityEvent OnDefeated;

    private void Start()
    {
        _scoreCounter = FindObjectOfType<ScoreCounter>();
        if (_scoreCounter != null) OnDefeated.AddListener(delegate { _scoreCounter.AddScore(scoreSO); });

        Physics2D.IgnoreLayerCollision(9, 9, true);

        _flySpeedy = Random.Range(_minFlydpeedy, _maxFlySpeedy);

        if (transform.position.x < 0)
        {
            FaceRight();
        }
        else
        {
            FaceLeft();
        }
    }

    private void Update()
    {
        if (transform.position.y < -8) Destroy(gameObject);
        if (Mathf.Abs(transform.position.x) >= 15) Destroy(gameObject);

        if (cc2D.IsTouching(GameState.ItemFilter))
        {
            CheckInteraction();
        }

        if (cc2D.IsTouching(GameState.GrassBlockFilter) && _isFlying) Walk();

        if (_isEating)
        {
            _eatCooldown -= Time.deltaTime;
            _eatSoundCooldown -= Time.deltaTime;

            if (_eatSoundCooldown <= 0)
            {
                audioSourceVoice.PlayOneShot(chewSFX, GameSettings.volumeSFX);
                _eatSoundCooldown = 1f;
            }

            if (_eatCooldown <= 0)
            {
                _eatCooldown = 5f;
                EatSeed();
            }
        }
    }

    private void FixedUpdate()
    {
        switch (_movementType)
        {
            case MovementType.Walk:
                Walk();
                break;

            case MovementType.Fly:
                Fly();
                break;

            case MovementType.Stop:
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void CheckInteraction()
    {
        cc2D.GetContacts(GameState.ItemFilter, _collider2Ds);

        if (_collider2Ds.Count == 0) return;

        foreach (var c in _collider2Ds)
        {
            if (c.GetComponent<Seed>() != null)
            {
                CatchSeed(c);
            }
            else
            {
                Die();
            }
        }
    }

    private void CatchSeed(Collider2D seed)
    {
        if (transform.childCount > 0)
        {
            Die();
            return;
        }

        var seedTransform = seed.GetComponent<Transform>();
        var seedRb = seed.GetComponent<Rigidbody2D>();
        var seedC2d = seed.GetComponent<Collider2D>();

        Physics2D.IgnoreCollision(cc2D, seedC2d, true);

        seedC2d.enabled = false;

        seedTransform.parent = transform;
        var position = transform.position;

        seedTransform.position = new Vector3(position.x, position.y - 0.5f, position.z);
        seedTransform.Rotate(0, 0, 90);

        seedRb.velocity = Vector2.zero;
        seedRb.angularVelocity = 0f;
        seedRb.bodyType = RigidbodyType2D.Kinematic;

        animator.SetBool(IsEating, true);
        _isEating = true;

        audioSourceVoice.PlayOneShot(catchSFX, GameSettings.volumeSFX);

        if (_movementType == MovementType.Walk) _movementType = MovementType.Stop;
    }

    public void InitializeWalk()
    {
        _movementType = MovementType.Walk;
    }

    public void InitializeFly()
    {
        _movementType = MovementType.Fly;
    }

    private void Walk()
    {
        if (_isFlying)
        {
            _isFlying = false;
            animator.SetTrigger(Land);
            _movementType = MovementType.Walk;
            if (_isEating) _movementType = MovementType.Stop;
            WingSoundConrtol(false);
        }

        animator.speed = Mathf.Abs(rb.velocity.x);

        int directionMultiplier = _isFacingRight ? 1 : -1;

        rb.AddForce(new Vector2(5 * directionMultiplier, 0f));
        if (Mathf.Abs(rb.velocity.x) > _maxWalkSpeed) rb.velocity = new Vector2(_maxWalkSpeed * directionMultiplier, rb.velocity.y);
    }

    private void WingSoundConrtol(bool shouldPlay)
    {
        if (wingSoundIsPlaying == shouldPlay) return;

        wingSoundIsPlaying = shouldPlay;

        if (wingSoundIsPlaying)
        {
            audioSourceWings.clip = wingsSFX;
            audioSourceWings.loop = true;
            audioSourceWings.Play();
        }
        else
        {
            audioSourceWings.Stop();
        }
    }

    private void Fly()
    {
        if (!_isFlying)
        {
            _isFlying = true;
            animator.SetTrigger(Fly1);
            _movementType = MovementType.Fly;
        }

        WingSoundConrtol(true);

        int directionMultiplier = _isFacingRight ? 1 : -1;
        float eatMultiplier = _isEating ? 0.5f : 1f;
        float eatWeight = _isEating ? 0.5f : 0f;

        rb.velocity = new Vector2(_maxFlySpeedx * directionMultiplier * eatMultiplier, _flySpeedy - eatWeight);
    }

    private void FaceLeft()
    {
        _isFacingRight = false;
        transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
    }

    private void FaceRight()
    {
        _isFacingRight = true;
        transform.rotation = new Quaternion(0f, 180f, 0f, 0f);
    }

    private void Die()
    {
        _isDead = true;
        _isEating = false;

        OnDefeated.Invoke();

        _movementType = MovementType.Stop;
        rb.bodyType = RigidbodyType2D.Dynamic;
        cc2D.isTrigger = true;
        animator.SetTrigger(Die1);
        DropSeed();

        WingSoundConrtol(false);

        audioSourceVoice.PlayOneShot(deathSFX, GameSettings.volumeSFX);
        audioSourceWings.Stop();

        rb.freezeRotation = false;
        float randTorque = Random.Range(5f, 8f);
        int directionMultiplier = Random.Range(0, 2) == 0 ? 1 : -1;

        rb.AddTorque(randTorque * directionMultiplier);
        rb.AddForce(new Vector2(0, 200));
    }

    private void DropSeed()
    {
        foreach (var child in transform.GetComponentsInChildren<Item>())
        {
            UnChildItem(child);
            child.BounceUniversal();
        }
    }

    private void UnChildItem(Item item)
    {
        var childRb = item.GetComponent<Rigidbody2D>();
        var childC2d = item.GetComponent<Collider2D>();

        childC2d.enabled = true;
        childRb.bodyType = RigidbodyType2D.Dynamic;

        var position = transform.position;
        var itemTransform = item.transform;
        itemTransform.position = new Vector3(position.x, position.y, 11f);

        itemTransform.parent = null;
    }

    private void EatSeed()
    {
        if (transform.childCount == 0) return;
        transform.GetChild(0).GetComponent<Seed>().GetEaten();
        animator.SetBool(IsEating, false);
        _isEating = false;
        if (_movementType == MovementType.Stop) _movementType = MovementType.Walk;
    }
}