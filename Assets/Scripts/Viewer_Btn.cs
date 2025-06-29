using UnityEngine;
using UnityEngine.SceneManagement;

public class Viewer_Btn : MonoBehaviour
{
    public string startSceneName = "3DViewer"; 

    public void LoadStartScene()
    {
        SceneManager.LoadSceneAsync(startSceneName);
    }
}