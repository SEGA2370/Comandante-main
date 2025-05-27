using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverPanel : MonoBehaviour
{

    [SerializeField] GameObject gameOverPanel;
    [SerializeField] private HealthPoints healthPoints;

    void Start()
    {
        healthPoints.HealthChanged.AddListener(GameOverPanelUpdate); // Subscribe to the healthChanged event
    }


    private void GameOverPanelUpdate()
    {
       if (healthPoints.CurrentHealth <= 0)
        {
            gameOverPanel.SetActive(true);
        }
       else
        {
            gameOverPanel.SetActive(false);
        }

    }
}
