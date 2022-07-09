using System.Collections.Generic;
using UnityEngine;

public class SunflowerPart : MonoBehaviour
{
    [SerializeField] private List<Sprite> sprites;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private bool _isGrowing;
    private bool _isWatered;
    private bool _isInfested;

    private int _maxSpriteIndex;
    private int _currentSpriteIndex;

    private float _timer;

    private float _growCooldown;
    private float _standardCooldown = 1f;
    private float _wateredCooldown = 0.2f;
    private float _infestedCooldown = 2f;
    private float _wateredAndInfestedCooldown = 1.5f;

    private void Start()
    {
        _timer = 0;

        _growCooldown = DetermineCooldown(_isWatered, _isInfested);
        _isGrowing = true;
        _currentSpriteIndex = 0;
        _maxSpriteIndex = sprites.Count - 1;

        spriteRenderer.sprite = sprites[_currentSpriteIndex];
    }

    private void Update()
    {
        if (_isGrowing) _timer += Time.deltaTime;

        if (_timer >= _growCooldown)
        {
            _timer = 0;
            Grow();
        }
    }

    private float DetermineCooldown(bool waterBool, bool infestBool)
    {
        if (waterBool && infestBool) return _wateredAndInfestedCooldown;
        if (waterBool && !infestBool) return _wateredCooldown;
        if (!waterBool && infestBool) return _infestedCooldown;
        return _standardCooldown;
    }

    private void Grow()
    {
        _currentSpriteIndex++;
        spriteRenderer.sprite = sprites[_currentSpriteIndex];

        if (_currentSpriteIndex >= _maxSpriteIndex)
        {
            _isGrowing = false;
            AlertParent();
        }
    }

    private void AlertParent()
    {
        transform.parent.GetComponent<Sunflower>().SegmentFinish();
    }

    public void ParentInitialize(int height, bool wateredBool, bool infestBool)
    {
        transform.localPosition = new Vector3(0, height, 0);
        _isWatered = wateredBool;
        _isInfested = infestBool;
    }

    public void SetIsWatered(bool toSet)
    {
        _isWatered = toSet;
        _growCooldown = DetermineCooldown(_isWatered, _isInfested);
    }

    public void SetIsInfested(bool toSet)
    {
    }
}