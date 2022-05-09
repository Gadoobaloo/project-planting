using TMPro;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMeshProUGUI;

    private float _timerStartTime;

    private void Start()
    {
        _timerStartTime = 300.0f;
    }

    private void Update()
    {
        if (_timerStartTime > 0)
        {
            _timerStartTime -= Time.deltaTime;
        }

        DisplayTime(_timerStartTime);
    }

    private void DisplayTime(float toDisplay)
    {
        if (toDisplay < 0)
        {
            toDisplay = 0;
        }

        float minutes = Mathf.FloorToInt(toDisplay / 60);
        float seconds = Mathf.FloorToInt(toDisplay % 60);

        textMeshProUGUI.text = $"{minutes:00}:{seconds:00}";
    }

    public float GetTimeRemaining()
    {
        return _timerStartTime;
    }
}