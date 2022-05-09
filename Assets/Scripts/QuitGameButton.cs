using UnityEngine;

public class QuitGameButton : MonoBehaviour
{
    public void Activate()
    {
        Debug.Log("Closed Game");
        Application.Quit();
    }
}
