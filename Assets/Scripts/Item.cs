using UnityEngine;
using Random = UnityEngine.Random;

public abstract class Item : MonoBehaviour
{
    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] protected SpriteRenderer spriteRenderer;
    [SerializeField] protected Collider2D c2D;

    protected ContactFilter2D FilterGrassBlock;

    protected bool IsLanded;

    private void Start()
    {
        FilterGrassBlock.SetDepth(12f, 12f);
        IsLanded = false;


        if (transform.position.x <= 0)
        {
            //seed is on left
            Launch(200, 600, 100, 400);
        }
        else
        {
            //seed is on right
            Launch(-300, -600, 150, 250);
        }
    }

    private void Update()
    {
        if(transform.position.y < -8) Destroy(gameObject);

        if (c2D.IsTouching(FilterGrassBlock) && !IsLanded)
        {
            IsLanded = true;
            spriteRenderer.color = new Color(1f, 1f, 1f, 0.5f);
            Destroy(gameObject, 0.5f);
        }
    }

    public abstract void Activate();

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
    
    
    
}