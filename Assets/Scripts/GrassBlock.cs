using System;
using UnityEngine;
using UnityEngine.Events;

public enum DirtState
{ None, Small, Medium, Fertile, Planted }

public class GrassBlock : MonoBehaviour
{
    private MyPlayerControls _controls;

    [SerializeField] private Transform sunflowerPrefab;
    [SerializeField] private SpriteRenderer spriteRendererShovel;
    [SerializeField] private SpriteRenderer spriteRendererDirt;
    [SerializeField] private Animator shovelAnimator;
    [SerializeField] private Sprite[] dirtStateSprites;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip digSFX;
    [SerializeField] private AudioClip plantSFX;
    [SerializeField] private AudioClip waterPlantSFX;

    private float specialDigPitch = 1.15f;

    [SerializeField] private ScoreSO seedPlantedScoreSO;
    [SerializeField] private ScoreSO blockFertilizedScoreSO;
    [SerializeField] private ScoreSO plantsWateredScoreSO;

    private ScoreCounter _scoreCounter;

    public DirtState CurrenDirtState { get; private set; }

    private float _timer;

    private Sunflower _sunflower;

    private bool _isDigReady;
    private bool _isDigging;

    public bool IsGrowingFlower => _sunflower != null && _sunflower.IsGrowing;

    public bool HasGrownFlower => _sunflower != null && _sunflower.HasGrown;

    [SerializeField] private UnityEvent OnSeedPlanted;
    [SerializeField] private UnityEvent OnBlockFertilized;
    [SerializeField] private UnityEvent OnWateredFlower;

    private void Awake()
    {
        _controls = new MyPlayerControls();
    }

    private void Start()
    {
        _scoreCounter = FindObjectOfType<ScoreCounter>();
        if (_scoreCounter != null)
        {
            OnSeedPlanted.AddListener(delegate { _scoreCounter.AddScore(seedPlantedScoreSO); });
            OnBlockFertilized.AddListener(delegate { _scoreCounter.AddScore(blockFertilizedScoreSO); });
            OnWateredFlower.AddListener(delegate { _scoreCounter.AddScore(plantsWateredScoreSO); });
        }

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
            _timer += Time.unscaledDeltaTime;
        }

        if (_timer >= 0.7f)
        {
            TimerAdvanceDirtState();
            _timer = 0f;
        }
    }

    public void HideShovel()
    {
        _isDigReady = false;
        if (_isDigging) DigStop();
        spriteRendererShovel.color = new Color(1f, 1f, 1f, 0f);
    }

    public void ShowShovel()
    {
        if (CurrenDirtState != DirtState.Fertile & CurrenDirtState != DirtState.Planted)
        {
            _isDigReady = true;
            spriteRendererShovel.color = new Color(1f, 1f, 1f, 1f);
        }
    }

    private void DigStart()
    {
        if (_isDigReady == false) return;

        _isDigging = true;
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
        CurrenDirtState = toSet;

        switch (CurrenDirtState)
        {
            case (DirtState.None):
                spriteRendererDirt.sprite = dirtStateSprites[0];
                spriteRendererDirt.color = new Color(1f, 1f, 1f, 0f);
                break;

            case DirtState.Small:
                spriteRendererDirt.sprite = dirtStateSprites[0];
                audioSource.PlayOneShot(digSFX, GameSettings.volumeSFX);
                spriteRendererDirt.color = new Color(1f, 1f, 1f, 1f);
                break;

            case DirtState.Medium:
                spriteRendererDirt.sprite = dirtStateSprites[1];
                audioSource.PlayOneShot(digSFX, GameSettings.volumeSFX);
                spriteRendererDirt.color = new Color(1f, 1f, 1f, 1f);
                break;

            case DirtState.Fertile:
                spriteRendererDirt.sprite = dirtStateSprites[2];

                audioSource.pitch = specialDigPitch;
                audioSource.PlayOneShot(digSFX, GameSettings.volumeSFX);

                spriteRendererDirt.color = new Color(1f, 1f, 1f, 1f);
                OnBlockFertilized.Invoke();
                break;

            case DirtState.Planted:
                spriteRendererDirt.sprite = dirtStateSprites[3];
                spriteRendererDirt.color = new Color(1f, 1f, 1f, 1f);
                audioSource.PlayOneShot(plantSFX, GameSettings.volumeSFX);
                GrowSunflower();
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(toSet), toSet, null);
        }
    }

    private void TimerAdvanceDirtState()
    {
        switch (CurrenDirtState)
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
        return CurrenDirtState == DirtState.Fertile;
    }

    public void PlantSeed()
    {
        SetDirtState(DirtState.Planted);
    }

    public void ShovelItemLand()
    {
        if (CurrenDirtState != DirtState.Planted)
        {
            SetDirtState(DirtState.Fertile);
        }
    }

    private void GrowSunflower()
    {
        OnSeedPlanted.Invoke();

        var flower = Instantiate(sunflowerPrefab, transform);

        _sunflower = flower.GetComponent<Sunflower>();
    }

    public void SpeedUpFlowerGrowth()
    {
        if (_sunflower != null)
        {
            if (_sunflower.IsWatered) return;

            _sunflower.Water();
            audioSource.PlayOneShot(waterPlantSFX, GameSettings.volumeSFX);
            OnWateredFlower.Invoke();
        }
    }

    public void HarvestSunflower()
    {
        if (GameState.GameMode == GameMode.Standard) return;

        _sunflower.Harvest();
        audioSource.PlayOneShot(plantSFX, GameSettings.volumeSFX);
        _sunflower = null;
        SetDirtState(DirtState.None);
    }
}