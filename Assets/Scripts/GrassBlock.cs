using System;
using UnityEngine;

enum DirtState {None, Small, Medium, Fertile, Planted}

public class GrassBlock : MonoBehaviour
{
    private MyPlayerControls _controls;

    [SerializeField] private Transform sunflowerPrefab;
    [SerializeField] private SpriteRenderer spriteRendererShovel;
    [SerializeField] private SpriteRenderer spriteRendererDirt;
    [SerializeField] private Animator shovelAnimator;
    [SerializeField] private Sprite[] dirtStateSprites;
    
    private Sunflower _sunflower;

    private bool _isDigReady;
    private bool _isDigging;
    private bool _isGrowingFlower;

    private DirtState _currentDirtState;
    private float _timer;

    private void Awake()
    {
        _controls = new MyPlayerControls();
    }

    private void Start()
    {
        _isDigReady = false;
        _isDigging = false;

        SetDirtState(0);

        shovelAnimator.speed = 0f;
        HideShovel();
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
        _controls.Glove.PrimaryButton.performed += ctx => DigStart();
        _controls.Glove.PrimaryButton.canceled += ctx => DigStop();

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
        if(_isDigReady == false) return;
        
        _isDigging = true;
        
        //todo- have animation speed change based on game state
        shovelAnimator.speed = 1f;
    }

    private void DigStop()
    {
        if (!_isDigging) return;
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
                GrowSunflower();
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

    private void GrowSunflower()
    {
        var flower = Instantiate(sunflowerPrefab, transform);
        _isGrowingFlower = true;

        _sunflower = flower.GetComponent<Sunflower>();
    }

    public void SpeedUpFlowerGrowth()
    {
        if(_sunflower != null) _sunflower.Water();
    }

    public void HarvestSunflower()
    {
        // have the sunflower be uprooted and despawn
    }

    public bool GetIsGrowingFlower()
    {
        return _isGrowingFlower;
    }
}
