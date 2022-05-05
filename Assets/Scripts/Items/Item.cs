using UnityEngine;

public abstract class Item : MonoBehaviour
{
    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] protected SpriteRenderer spriteRenderer;
    [SerializeField] protected Collider2D c2D;

    protected ContactFilter2D FilterGrassBlock;

    private bool _isLanded;
    private bool _isRising;
    private bool _isFalling;
    private bool _isActivated;

    private void Start()
    {
        FilterGrassBlock.SetDepth(12f, 12f);
    }

    private void Update()
    {
        if(transform.position.y < -8) Destroy(gameObject);
        
        if (rb.velocity.y > 0 && !_isRising) RiseUniversal();
        if (rb.velocity.y < 0 && !_isFalling) FallUniversal();
        if (c2D.IsTouching(FilterGrassBlock) && !_isActivated) LandUniversal();
    }

    protected void Launch(float x, float y)
    {
        rb.AddForce(new Vector2(x, y));
    }

    protected void Launch(float xMin, float xMax, float yMin, float yMax)
    {
        var x = Random.Range(xMin, xMax);
        var y = Random.Range(yMin, yMax);
        
        rb.AddForce(new Vector2(x, y));
    }

    private void RiseUniversal()
    {
        _isRising = true;
        _isFalling = false;
        _isLanded = false;
        
        RiseCustom();
    }

    private void FallUniversal()
    {
        _isFalling = true;
        _isRising = false;
        _isLanded = false;
        
        FallCustom();
    }

    private void LandUniversal()
    {
        _isRising = false;
        _isFalling = false;
        _isLanded = true;

        LandCustom();
    }

    public void ActivateUniversal()
    {
        _isActivated = true;
         
        ActivateCustom();
    }

    public void DestroyUniversal()
    {
        DestroyCustom();
    }

    public bool GetIsRising()
    {
        return _isRising;
    }

    protected void SetIsRising(bool toSet)
    {
        _isRising = toSet;
    }

    public bool GetIsFalling()
    {
        return _isFalling;
    }
    
    public bool GetIsLanded()
    {
        return _isLanded;
    }

    protected void SetIsLanded(bool toSet)
    {
        _isLanded = toSet;
    }

    public bool GetIsActivated()
    {
        return _isActivated;
    }

    protected void SetIsActivated(bool toSet)
    {
        _isActivated = toSet;
    }

    protected abstract void ActivateCustom();
    protected abstract void RiseCustom();
    protected abstract void FallCustom();
    protected abstract void LandCustom();
    public abstract void SpawnLaunch();
    public abstract void BounceCustom();
    protected abstract void DestroyCustom();
}