using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeButtonController : MonoBehaviour
{
    public string startSceneName = "Startseite"; 

    public void LoadStartScene()
    {
        SceneManager.LoadSceneAsync(startSceneName);
    }
}