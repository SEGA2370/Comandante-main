using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] GameObject prevScene;
    [SerializeField] GameObject optionPanel;

    void Awake()
    {
        // Always start unpaused and hide pause panel
        Time.timeScale = 1f;
        if (optionPanel != null) optionPanel.SetActive(false);
    }

    private void Update()
    {
        PrevLoader();

        if (Input.GetKeyDown(KeyCode.Escape))
            TogglePause();
    }

    public void TogglePause()
    {
        if (optionPanel == null) return;

        bool opening = !optionPanel.activeSelf;
        optionPanel.SetActive(opening);
        Time.timeScale = opening ? 0f : 1f;

        Debug.Log($"[SceneLoader] TogglePause – {(opening ? "OPEN" : "CLOSED")}, timeScale={Time.timeScale}");
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

        int sceneToLoad = SaveSystem.HasSavedProgress()
            ? SaveSystem.LoadLevelProgress()
            : 1;

        SceneManager.LoadScene(sceneToLoad);
    }

    public void ResetProgressAndRestart()
    {
        SaveSystem.ResetProgress();
        Time.timeScale = 1f;                         // <<< ensure unpaused
        SceneManager.LoadScene(1);
    }

    public void GoToMenu()
    {
        Time.timeScale = 1f;                         // <<< ensure unpaused
        SceneManager.LoadScene(0);
    }

    public void RestartGame()
    {
        int current = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
    Time.timeScale = 1f; // ensure unpaused
    AdsManager.I?.ShowInterstitial(() =>
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(current);
    });
    }

    public void LoadNextSceneWithRewarded()
    {
        int current = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        int next = current + 1;

        SaveSystem.SaveLevelProgress(next);
        Time.timeScale = 1f;

        // Если AdsManager вообще нет – просто грузим следующий уровень
        if (AdsManager.I == null)
        {
            SceneManager.LoadScene(next);
            return;
        }

        AdsManager.I.ShowRewarded(success =>
        {
            // В ЛЮБОМ случае переходим на следующий уровень
            SceneManager.LoadScene(next);
        });
    }

    public void LoadNextScene()
    {
        int current = SceneManager.GetActiveScene().buildIndex;
        int next = current + 1;

        SaveSystem.SaveLevelProgress(next);
        Time.timeScale = 1f;                         // <<< ensure unpaused
        SceneManager.LoadScene(next);                // без рекламы
    }

    public void LoadPreviousScene()
    {
        int current = SceneManager.GetActiveScene().buildIndex;
        int prev = current - 1;

        Time.timeScale = 1f;                         // <<< ensure unpaused
        SceneManager.LoadScene(prev);                // без рекламы
    }

    public void PrevLoader()
    {
        if (SceneManager.GetActiveScene().name == "First_Level" && prevScene != null)
            prevScene.SetActive(false);
    }
}
