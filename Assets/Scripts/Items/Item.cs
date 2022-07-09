using UnityEngine;

public abstract class Item : MonoBehaviour
{
    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] protected SpriteRenderer spriteRenderer;
    [SerializeField] protected Collider2D c2D;

    [SerializeField] protected Transform offscreenTF;
    [SerializeField] protected SpriteRenderer offscreenSR;

    [SerializeField] protected AudioSource audioSource;
    [SerializeField] protected AudioClip clankSFX;

    public bool IsLanded { get; set; }
    public bool IsRising { get; set; }
    public bool IsFalling { get; set; }
    public bool IsActivated { get; set; }

    private void Update()
    {
        if (transform.position.y < -8) Destroy(gameObject);

        if (rb.velocity.y > 0 && !IsRising) RiseUniversal();
        if (rb.velocity.y < 0 && !IsFalling) FallUniversal();
        if (c2D.IsTouching(GameState.GrassBlockFilter) && !IsActivated) LandUniversal();
        if (c2D.IsTouching(GameState.ItemFilter)) ClankUniversal();

        if (IsFalling && !GetComponent<Renderer>().isVisible && transform.position.y > 0)
        {
            ShowIcon(true);
        }
        else
        {
            ShowIcon(false);
        }
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
        IsRising = true;
        IsFalling = false;
        IsLanded = false;

        RiseCustom();
    }

    private void FallUniversal()
    {
        IsFalling = true;
        IsRising = false;
        IsLanded = false;

        FallCustom();
    }

    private void LandUniversal()
    {
        IsRising = false;
        IsFalling = false;
        IsLanded = true;

        LandCustom();
    }

    public void ActivateUniversal()
    {
        IsActivated = true;
        audioSource.PlayOneShot(clankSFX, GameSettings.volumeSFX);

        ActivateCustom();
    }

    public void BounceUniversal()
    {
        BounceCustom();
    }

    public void DestroyUniversal()
    {
        DestroyCustom();
    }

    private void ClankUniversal()
    {
        if (!audioSource.isPlaying) audioSource.PlayOneShot(clankSFX, GameSettings.volumeSFX);
    }

    private void ShowIcon(bool show)
    {
        offscreenSR.enabled = show;

        offscreenTF.SetPositionAndRotation(new Vector3(transform.position.x, 5.3f, 0f), Quaternion.identity);
    }

    protected abstract void ActivateCustom();

    protected abstract void RiseCustom();

    protected abstract void FallCustom();

    protected abstract void LandCustom();

    public abstract void SpawnLaunch();

    protected abstract void BounceCustom();

    protected abstract void DestroyCustom();
}