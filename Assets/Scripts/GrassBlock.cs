using System;
using UnityEngine;

enum DirtState {None, Small, Medium, Fertile, Planted}

public class GrassBlock : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRendererShovel;
    [SerializeField] private SpriteRenderer spriteRendererDirt;

    [SerializeField] private Animator shovelAnimator;

    [SerializeField] private Sprite[] dirtStateSprites;
    
    private bool _isDigReady;
    private bool _isDigging;

    private DirtState _currentDirtState;
    private float _timer;

    private void Start()
    {
        _isDigReady = false;
        _isDigging = false;

        SetDirtState(0);

        shovelAnimator.speed = 0f;
        HideShovel();
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && _isDigReady && !_isDigging)
        {
            DigStart();
        }

        if (Input.GetMouseButtonUp(0) && _isDigging)
        {
            DigStop();
        }

        if (_isDigging)
        {
            _timer += Time.deltaTime;
        }

        if (_timer >= 1f)
        {
            TimerAdvanceDirtState();
            _timer = 0f;
        }
    }

    public void HideShovel()
    {
        _isDigReady = false;
        if(_isDigging) DigStop();
        spriteRendererShovel.color = new Color(1f, 1f, 1f, 0f);
    }

    public void ShowShovel()
    {
        if (_currentDirtState != DirtState.Fertile & _currentDirtState != DirtState.Planted)
        {
            _isDigReady = true;
            spriteRendererShovel.color = new Color(1f, 1f, 1f, 1f);
        }
    }

    private void DigStart()
    {
        _isDigging = true;
        
        //todo- have animation speed change based on game state
        shovelAnimator.speed = 1f;
    }

    private void DigStop()
    {
        _isDigging = false;
        
        shovelAnimator.Play("shovelDiggingAnimation", 0, 0);
        shovelAnimator.speed = 0f;
    }

    private void SetDirtState(DirtState toSet)
    {
        _currentDirtState = toSet;
        
        switch (_currentDirtState)
        {
            case (DirtState.None):
                spriteRendererDirt.sprite = dirtStateSprites[0];
                spriteRendererDirt.color = new Color(1f, 1f, 1f, 0f);
                break;
            case DirtState.Small:
                spriteRendererDirt.sprite = dirtStateSprites[0];
                spriteRendererDirt.color = new Color(1f, 1f, 1f, 1f);
                break;
            case DirtState.Medium:
                spriteRendererDirt.sprite = dirtStateSprites[1];
                spriteRendererDirt.color = new Color(1f, 1f, 1f, 1f);
                break;
            case DirtState.Fertile:
                spriteRendererDirt.sprite = dirtStateSprites[2];
                spriteRendererDirt.color = new Color(1f, 1f, 1f, 1f);
                break;
            case DirtState.Planted:
                spriteRendererDirt.sprite = dirtStateSprites[3];
                spriteRendererDirt.color = new Color(1f, 1f, 1f, 1f);
                
                //todo - activate sunflower
                Debug.Log("grow sunflower");
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(toSet), toSet, null);
        }
    }

    private void TimerAdvanceDirtState()
    {
        switch (_currentDirtState)
        {
            case DirtState.None:
                SetDirtState(DirtState.Small);
                break;
            case DirtState.Small:
                SetDirtState(DirtState.Medium);
                break;
            case DirtState.Medium:
                SetDirtState(DirtState.Fertile);
                HideShovel();
                break;
            default:
                HideShovel();
                break;
        }
    }

    public bool GetIsFertile()
    {
        return _currentDirtState == DirtState.Fertile;
    }

    public void PlantSeed()
    {
        SetDirtState(DirtState.Planted);
    }

    public void ShovelItemLand()
    {
        SetDirtState(DirtState.Fertile);
    }
}
