using System;
using System.Collections.Generic;
using UnityEngine;

enum MovementType { Walk, Fly, Stop }

public class BugEnemy : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private CapsuleCollider2D cc2D;
    [SerializeField] private Animator _animator;

    private List<Collider2D> _collider2Ds = new List<Collider2D>();
    private ContactFilter2D _itemFilter;
    private ContactFilter2D _grassBlockFilter;

    private bool _isMidAir;
    private bool _isFlying;
    private bool _isEating;

    private float _eatCooldown = 5f;

    private MovementType _movementType;
    private float _maxWalkSpeed = 1f;
    private float _maxFlySpeedx = 2f;
    private float _maxFlySpeedy = 1f;
    
    private static readonly int IsEating = Animator.StringToHash("isEating");
    private static readonly int Die1 = Animator.StringToHash("Die");
    private static readonly int Land = Animator.StringToHash("Land");
    private static readonly int Fly1 = Animator.StringToHash("Fly");

    private void Start()
    {
        _itemFilter.SetDepth(11f, 11f);
        _grassBlockFilter.SetDepth(12f, 12f);
        
        //determine what direction to walk in
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
        if(transform.position.y < -8) Destroy(gameObject);

        if (cc2D.IsTouching(_itemFilter))
        {
            CheckInteraction();
        }

        if (_isEating)
        {
            _eatCooldown -= Time.deltaTime;

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
        cc2D.GetContacts(_itemFilter, _collider2Ds);

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

        seedTransform.parent = transform;
        var position = transform.position;
        
        seedTransform.position = new Vector3(position.x, position.y - 0.5f, position.z);
        seedTransform.Rotate(0,0,90);
        
        seedRb.velocity = Vector2.zero;
        seedRb.angularVelocity = 0f;
        seedRb.bodyType = RigidbodyType2D.Kinematic;

        _animator.SetBool(IsEating, true);
        _isEating = true;
    }

    public void InitializeWalk()
    {
        _movementType = MovementType.Walk;
    }
    
    private void Walk()
    {
        if (_isFlying)
        {
            _isFlying = false;
            _animator.SetTrigger(Land);
        }

        rb.AddForce(new Vector2(-5, 0f));
        if (Mathf.Abs(rb.velocity.x) > _maxWalkSpeed) rb.velocity = new Vector2(-_maxWalkSpeed, rb.velocity.y);
    }

    public void InitializeFly()
    {
        _movementType = MovementType.Fly;
    }

    private void Fly()
    {
        if (!_isFlying)
        {
            _isFlying = true;
            _animator.SetTrigger(Fly1);
        }

        rb.velocity = new Vector2(_maxFlySpeedx, _maxFlySpeedy);
        //make it go up a minimum and maximum height
        //have it
    }

    private void FaceLeft()
    {
        transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
    }

    private void FaceRight()
    {
        transform.rotation = new Quaternion(0f, 180f, 0f, 0f);
    }
    
    private void Die()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
        cc2D.isTrigger = true;
        _animator.SetTrigger(Die1);
        DropSeed();
        rb.AddForce(new Vector2(0, 200));
    }

    private void DropSeed()
    {
        foreach (var child in transform.GetComponentsInChildren<Item>())
        {
            UnchildItem(child);
            child.Bounce();
        }
    }

    private void UnchildItem(Item item)
    {
        var childRb = item.GetComponent<Rigidbody2D>();
        childRb.bodyType = RigidbodyType2D.Dynamic;

        var position = transform.position;
        var itemTransform = item.transform;
        itemTransform.position = new Vector3(position.x, position.y, 11f);

        itemTransform.parent = null;
    }

    private void EatSeed()
    {
        if (transform.childCount == 0) return;
        Destroy(transform.GetChild(0).gameObject);
        _animator.SetBool(IsEating, false);
        _isEating = false;
    }
}
