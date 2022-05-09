using System;
using TMPro;
using UnityEngine;

public class Counter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMeshProUGUI;
    private int _counterValue;

    private void Start()
    {
        SetCounterValue(0);

        Sunflower.OnSunflowerGrown += AdvanceCounter;
    }

    private void AdvanceCounter(object sender, EventArgs e)
    {
        SetCounterValue(_counterValue + 1);
    }

    private void SetCounterValue(int toSet)
    {
        _counterValue = toSet;
        textMeshProUGUI.text = _counterValue.ToString();
    }
}