using UnityEngine;
using UnityEngine.SceneManagement;

public class Quiz_Btn : MonoBehaviour
{
    public string startSceneName = "QuizModus"; 

    public void LoadStartScene()
    {
        SceneManager.LoadSceneAsync(startSceneName);
    }
}