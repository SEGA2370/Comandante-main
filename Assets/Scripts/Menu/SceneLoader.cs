using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using YG;
public class SceneLoader : MonoBehaviour
{
    [SerializeField] GameObject prevScene;
    [SerializeField] GameObject optionPanel;

    private void Update()
    {
        PrevLoader();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }
    public void TogglePause()
    {
        // flip panel visibility
        bool opening = !optionPanel.activeSelf;
        optionPanel.SetActive(opening);

        // pause when opening, resume when closing
        Time.timeScale = opening ? 0f : 1f;

        Debug.Log($"[SceneLoader] TogglePause called – panel is now {(opening ? "OPEN" : "CLOSED")}, timeScale={Time.timeScale}");
    }
    public void PlayGame()
    {
        if (optionPanel != null && optionPanel.activeSelf)
        {
            optionPanel.SetActive(false);
            Time.timeScale = 1f;
            return;
        }

        Time.timeScale = 1f;

        int sceneToLoad = SaveSystem.HasSavedProgress() ? SaveSystem.LoadLevelProgress() : 1;
        SceneManager.LoadScene(sceneToLoad);
    }

    public void ResetProgressAndRestart()
    {
        SaveSystem.ResetProgress();
        SceneManager.LoadScene(1); // или SceneManager.GetActiveScene().buildIndex
    }

    public void GoToMenu()
    { 
        SceneManager.LoadScene(0);
    }
    public void RestartGame()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
    public void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        SaveSystem.SaveLevelProgress(nextSceneIndex);

        if (YG2.isTimerAdvCompleted)
        {
            YG2.InterstitialAdvShow();
            YG2.onCloseInterAdv += () =>
            {
                SceneManager.LoadScene(nextSceneIndex);
                YG2.onCloseInterAdv = null; // Отписаться
            };
        }
        else
        {
            // Слишком рано — пропустить показ и просто перейти
            SceneManager.LoadScene(nextSceneIndex);
        }
    }
    public void LoadPreviousScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int previousSceneIndex = currentSceneIndex - 1;
        if (YG2.isTimerAdvCompleted)
        {
            YG2.InterstitialAdvShow();
            YG2.onCloseInterAdv += () =>
            {
                SceneManager.LoadScene(previousSceneIndex);
                YG2.onCloseInterAdv = null; // Отписаться
            };
        }
        else
        {
            // Слишком рано — пропустить показ и просто перейти
            SceneManager.LoadScene(previousSceneIndex);
        }
    }
    public void PrevLoader()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        if (currentSceneName == "First_Level")
        {
            prevScene.gameObject.SetActive(false);
        }
    }
}
