using UnityEngine;

public class GrassBlock : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRendererShovel;
    [SerializeField] private SpriteRenderer spriteRendererDirt;
    
    [SerializeField] private Sprite[] dirtStateSprites;
    private bool _isFertile;

    private void Start()
    {
        SetDirtState(0);
        HideShovel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HideShovel()
    {
        spriteRendererShovel.color = new Color(1f, 1f, 1f, 0f);
    }

    public void ShowShovel()
    {
        spriteRendererShovel.color = new Color(1f, 1f, 1f, 1f);
    }

    void Dig()
    {
        //play shovel animation
        //have progress bar show
        //allow for cancel
        //after successful dig, apply fertilized soil state
    }

    private void SetDirtState(int toSet)
    {
        spriteRendererDirt.sprite = toSet == 0 ? null : dirtStateSprites[toSet - 1];
        _isFertile = toSet == 4;
    }

    public bool GetIsFertile()
    {
        return _isFertile;
    }
}
