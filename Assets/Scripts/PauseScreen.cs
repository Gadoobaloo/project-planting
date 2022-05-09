using UnityEngine;

public class PauseScreen : MonoBehaviour
{
    [SerializeField] private GameObject pauseScreen;
    public bool CanPause { get; set; }
    private bool _isPaused;

    private MyPlayerControls _controls;

    private void Awake()
    {
        _controls = new MyPlayerControls();
    }

    private void Start()
    {
        ResumeGame();
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
        _controls.UI.PauseStart.performed += ctx => TogglePause();
    }

    private void TogglePause()
    {
        if (!_isPaused)
        {
            PauseGame();
        }
        else
        {
            ResumeGame();
        }
    }

    private void PauseGame()
    {
        _isPaused = true;
        pauseScreen.SetActive(true);
        Time.timeScale = 0f;
        Cursor.visible = true;
    }

    private void ResumeGame()
    {
        _isPaused = false;
        pauseScreen.SetActive(false);
        Time.timeScale = 1f;
        Cursor.visible = false;
    }
}