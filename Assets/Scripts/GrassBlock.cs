using UnityEngine;

public class GrassBlock : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRendererShovel;
    [SerializeField] private SpriteRenderer spriteRendererDirt;
    
    [SerializeField] private Sprite[] dirtStateSprites;
    private bool _isFertile;
    private bool _isDigReady;
    private bool _isDigging;

    private void Start()
    {
        _isFertile = false;
        _isDigReady = false;
        _isDigging = false;

        SetDirtState(0);
        HideShovel();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && _isDigReady)
        {
            DigStart();
        }

        if (Input.GetMouseButtonUp(0) && _isDigging)
        {
            DigStop();
        }
    }

    public void HideShovel()
    {
        _isDigReady = false;
        spriteRendererShovel.color = new Color(1f, 1f, 1f, 0f);
    }

    public void ShowShovel()
    {
        _isDigReady = true;
        spriteRendererShovel.color = new Color(1f, 1f, 1f, 1f);
    }

    void DigStart()
    {
        _isDigging = true;
    }

    private void DigStop()
    {
        _isDigging = false;
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
