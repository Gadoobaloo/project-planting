using System.Collections.Generic;
using UnityEngine;

public class Glove : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Collider2D circleCollider2D;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite[] spriteArray;

    private MyPlayerControls _controls;
    private float speed = 10;
    private Vector2 _controllerDirection;

    private ContactFilter2D _filterItems;
    private ContactFilter2D _filterGrassBlock;

    private bool _isGrabbing;
    private bool _isTouchGrass;

    private void Awake()
    {
        _controls = new MyPlayerControls();
    }

    private void Start()
    {
        _filterItems.SetDepth(11f, 11f);
        _filterGrassBlock.SetDepth(12f, 12f);

        Cursor.visible = false;

        _isGrabbing = false;
        _isTouchGrass = false;
    }

    private void OnEnable()
    {
        _controls.Enable();
    }

    private void OnDisable()
    {
        _controls.Disable();
    }

    private void Update()
    {
        _controls.Glove.MouseMove.performed += ctx => FollowMousePosition(ctx.ReadValue<Vector2>());
        _controls.Glove.KeysMove.performed += ctx => ControllerMovement(ctx.ReadValue<Vector2>());

        _controls.Glove.PrimaryButton.performed += ctx => Grab();
        _controls.Glove.PrimaryButton.canceled += ctx => DropItem();
        _controls.Glove.SecondaryButton.performed += ctx => LaunchItem();

        transform.Translate(_controllerDirection * speed * Time.deltaTime); //todo- reset controller direction when another input is detected
    }

    private void FollowMousePosition(Vector2 position)
    {
        var mousePos = mainCamera.ScreenToWorldPoint(position);

        transform.position = new Vector3(mousePos.x, mousePos.y, 10f);
    }

    private void ControllerMovement(Vector2 coordinates)
    {
        _controllerDirection = coordinates;
    }

    private void Grab()
    {
        if (_isTouchGrass) return;

        spriteRenderer.sprite = spriteArray[1];

        var collider2Ds = new List<Collider2D>();
        circleCollider2D.OverlapCollider(_filterItems, collider2Ds);

        foreach (var c2D in collider2Ds)
        {
            if (c2D.GetComponent<Item>() == null) continue;
            if (c2D.GetComponent<Item>().IsLanded) continue;
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
                c2D.GetComponent<Item>().BounceUniversal();
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
        if (!_isGrabbing) return;

        spriteRenderer.sprite = spriteArray[2];

        var allChildren = GetComponentsInChildren<Item>();
        foreach (var item in allChildren)
        {
            UnchildItem(item);
            item.ActivateUniversal();
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

    public void HideGlove()
    {
        _isTouchGrass = true;
        spriteRenderer.color = new Color(1f, 1f, 1f, 0f);
    }

    public void ShowGlove()
    {
        _isTouchGrass = false;
        spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
    }
}