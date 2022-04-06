using System;
using System.Collections.Generic;
using UnityEngine;

public class Glove : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite[] spriteArray;

    private Collider2D _circleCollider2D;
    
    private Vector3 _mousePosition;
    
    private ContactFilter2D _filterItems;
    private ContactFilter2D _filterGrassBlock;
    
    private bool _isGrabbing;
    private bool _isTouchGrass;
    
    private void Start()
    {
        _filterItems.SetDepth(11f, 11f);
        _filterGrassBlock.SetDepth(12f, 12f);
        
        Cursor.visible = false;
        _circleCollider2D = GetComponent<CircleCollider2D>();

        _isGrabbing = false;
        _isTouchGrass = false;
    }
    
    private void Update()
    {
        _mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        _mousePosition.z = 0;
        transform.position = _mousePosition;
        
        if (Input.GetMouseButtonDown(0) && !_isTouchGrass)
        {
            Grab();
        }

        if (Input.GetMouseButtonUp(0))
        {
            DropItem();
        }

        if (Input.GetMouseButtonDown(1) && _isGrabbing)
        {
            LaunchItem();
        }
        
        if (_circleCollider2D.IsTouching(_filterGrassBlock))
        {
            Debug.Log("here");
            //TouchGrass();
        }
    }
    
    private void Grab()
    {
        spriteRenderer.sprite = spriteArray[1];
        
        var collider2Ds = new List<Collider2D>();
        _circleCollider2D.OverlapCollider(_filterItems, collider2Ds);
        
        foreach (var c2D in collider2Ds)
        {
            if (c2D.GetComponent<Item>() == null) continue;
            if (c2D.GetComponent<Item>().GetIsLanded()) continue;
            if (!_isGrabbing)
            {
                var itemTransform = c2D.GetComponent<Transform>();
                var itemRb = c2D.GetComponent<Rigidbody2D>();
    
                itemTransform.parent = transform;
                itemTransform.position = itemTransform.parent.position;
    
                itemRb.bodyType = RigidbodyType2D.Kinematic;
                itemRb.velocity = Vector2.zero;
                itemRb.angularVelocity = 0f;
                    
                _isGrabbing = true;
            }
            else
            {
                c2D.GetComponent<Item>().Activate();
            }
        }
    }

    private void DropItem()
    {
        spriteRenderer.sprite = spriteArray[0];
        _isGrabbing = false;
        
        var allChildren = GetComponentsInChildren<Item>();
        foreach (var item in allChildren)
        {
            UnchildItem(item);
        }
    }

    private void LaunchItem()
    {
        spriteRenderer.sprite = spriteArray[2];

        var allChildren = GetComponentsInChildren<Item>();
        foreach (var item in allChildren)
        {
            UnchildItem(item);
            item.Activate();
        }
    }

    private void UnchildItem(Item item)
    {
        var crb = item.GetComponent<Rigidbody2D>();
        crb.bodyType = RigidbodyType2D.Dynamic;

        var position = transform.position;
        var itemTransform = item.transform;
        itemTransform.position = new Vector3(position.x, position.y, 11f);
        
        itemTransform.parent = null;
    }

    private void TouchGrass()
    {
        _isTouchGrass = true;
        if(_isGrabbing) LaunchItem();
        HideGlove();
        //show the shovel of the first block
        //allow for cl
    }

    private void UntouchGrass()
    {
        _isTouchGrass = false;
        ShowGlove();
    }

    public void HideGlove()
    {
        spriteRenderer.color = new Color(1f, 1f, 1f, 0f);
    }

    public void ShowGlove()
    {
        spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
    }
    
}
