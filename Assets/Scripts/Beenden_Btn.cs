using UnityEngine;

public class Beenden_Btn : MonoBehaviour
{
    public void ExitApplication()
    {
         Application.Quit();
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}