using UnityEngine;

public enum GameMode
{ Standard, Tutorial }

public class GameState : MonoBehaviour
{
    [SerializeField] private GameTimer _gameTimer;

    public static ContactFilter2D ItemFilter;
    public static ContactFilter2D GrassBlockFilter;

    private static bool IsTutorial { get; set; }

    private void Awake()
    {
        ItemFilter.SetDepth(11f, 11f);
        GrassBlockFilter.SetDepth(12f, 12f);
    }
}