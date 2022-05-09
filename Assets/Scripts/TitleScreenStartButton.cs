using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenStartButton : MonoBehaviour
{
    public void ChangeScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
}