using UnityEngine;

public class QuitButton : MonoBehaviour
{
    public void QuitGame()
    {
        // For Editor
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // For real builds (PC, Android, iOS)
        Application.Quit();

        // Optional: useful for Android to make sure app minimizes
        // Handheld.Quit(); // (deprecated but still works on some devices)
#endif
    }
}
