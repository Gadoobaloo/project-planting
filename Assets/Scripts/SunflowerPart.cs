using System.Collections.Generic;
using UnityEngine;

public class SunflowerPart : MonoBehaviour
{
    [SerializeField] private List<Sprite> sprites;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private bool _isGrowing;
    private float _timer;
    private float _growCooldown;
    private int _maxSpriteIndex;
    private int _currentSpriteIndex;

    private void Start()
    {
        _timer = 0;
        _growCooldown = 1f;
        _isGrowing = true;
        _currentSpriteIndex = 0;
        _maxSpriteIndex = sprites.Count - 1;

        spriteRenderer.sprite = sprites[_currentSpriteIndex];
    }

    private void Update()
    {
        if(_isGrowing) _timer += Time.deltaTime;

        if (_timer >= _growCooldown)
        {
            _timer = 0;
            Grow();
        }
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

    public void SetHeight(int toSet)
    {
        transform.localPosition = new Vector3(0, toSet, 0);
    }
}
