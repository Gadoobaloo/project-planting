using System.Collections.Generic;
using UnityEngine;

public class SunflowerStem : MonoBehaviour
{
    [SerializeField] private List<Sprite> sprites;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private float _timer;
    private int _maxSpriteIndex;
    private int _currentSpriteIndex;

    private void Start()
    {
        _timer = 0;
        _currentSpriteIndex = 0;
        _maxSpriteIndex = sprites.Count - 1;

        spriteRenderer.sprite = sprites[_currentSpriteIndex];
    }

    private void Update()
    {
        _timer += Time.deltaTime;

        if (_timer >= 1f)
        {
            Grow();
        }
    }

    private void Grow()
    {
        _currentSpriteIndex++;
        spriteRenderer.sprite = sprites[_currentSpriteIndex];
        if(_currentSpriteIndex >= _maxSpriteIndex) AlertParent();
    }

    private void AlertParent()
    {
        transform.parent.GetComponent<Sunflower>().SegmentFinish();
    }
}
