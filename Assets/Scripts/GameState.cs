using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public enum GameMode
{ Standard = 0, Timed = 1, Limited = 2 }

public class GameState : MonoBehaviour
{
    [SerializeField] private GameObject resultsScreenGO;
    [SerializeField] private GameTimer _gameTimer;
    [SerializeField] private UnityEvent OnGameEnd;

    public static GameMode GameMode { get; private set; }

    public static ContactFilter2D ItemFilter;
    public static ContactFilter2D GrassBlockFilter;

    private static bool IsTutorial { get; set; }

    private void Awake()
    {
        ItemFilter.SetDepth(11f, 11f);
        GrassBlockFilter.SetDepth(12f, 12f);
    }

    private void Start()
    {
        GameMode = ValueCarrier.GameMode;
    }

    public void OnStandardGameEnd()
    {
        OnGameEnd.Invoke();
        StartCoroutine(EndGame());
    }

    public void OnTimedGameEnd()
    {
        OnGameEnd.Invoke();
        StartCoroutine(EndGame());
    }

    public void OnLimitedGameEnd()
    {
        OnGameEnd.Invoke();
        StartCoroutine(EndGame());
    }

    private IEnumerator EndGame()
    {
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(2f);
        Cursor.visible = true;
        resultsScreenGO.SetActive(true);
    }
}