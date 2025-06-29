using UnityEngine;
using UnityEngine.SceneManagement;

public class Weiter_Btn : MonoBehaviour
{
    public void OnWeiterGeklickt()
    {
        SceneManager.LoadScene("QuizModus");
    }
}
