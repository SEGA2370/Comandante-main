using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        // If the options panel is currently open, treat this as “Continue”
        if (optionPanel != null && optionPanel.activeSelf)
        {
            // hide panel and un-pause
            optionPanel.SetActive(false);
            Time.timeScale = 1f;
        }
        else
        {
            Time.timeScale = 1f;
            SceneManager.LoadSceneAsync(1);
        }
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
        SceneManager.LoadScene(nextSceneIndex);
    }
    public void LoadPreviousScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int previousSceneIndex = currentSceneIndex - 1;
        SceneManager.LoadScene(previousSceneIndex);
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
