using System.Collections;
using TMPro;
using UnityEngine;

public class Countdown : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textBox;

    private float _timer = 4f;

    private void Start()
    {
        Time.timeScale = 0f;
        textBox.text = "READY";
    }

    private void Update()
    {
        if (_timer >= 0) _timer -= Time.unscaledDeltaTime;
        if (_timer <= 0f)
        {
            Time.timeScale = 1f;
            StartCoroutine(GoDisplay());
        }
    }

    private IEnumerator GoDisplay()
    {
        textBox.text = "GO";
        yield return new WaitForSecondsRealtime(1f);
        gameObject.SetActive(false);
    }
}