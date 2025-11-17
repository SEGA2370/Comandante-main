using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverPanel : MonoBehaviour
{

    [SerializeField] GameObject gameOverPanel;
    [SerializeField] private HealthPoints healthPoints;
    [SerializeField] private ReviveCooldown reviveCooldown;

    void Start()
    {
        healthPoints.HealthChanged.AddListener(GameOverPanelUpdate); // Subscribe to the healthChanged event
    }

    void OnDestroy()
    {
        if (healthPoints != null)
            healthPoints.HealthChanged.RemoveListener(GameOverPanelUpdate);
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

    public void OnReviveWithAd()
    {
        // Block if cooldown not ready
        if (reviveCooldown != null && !reviveCooldown.CanUse())
        {
            Debug.Log($"[Revive] Cooldown active: {reviveCooldown.RemainingSeconds()}s left");
            return;
        }

        const float ONE_HEART_HP = 10f;

        AdsManager.I?.ShowRewarded(success =>
        {
            if (!success) return;

            // Restore 1 heart and mark alive (your ForceRevive handles flag)
            healthPoints.ForceRevive(ONE_HEART_HP);

            // Re-enable movement on the same player object
            var pm = healthPoints.GetComponent<PlayerMovement>();
            if (pm == null) pm = healthPoints.GetComponentInParent<PlayerMovement>();
            if (pm != null) pm.enabled = true;

            // Hide game over and unpause
            if (gameOverPanel != null) gameOverPanel.SetActive(false);
            Time.timeScale = 1f;

            // Start cooldown
            reviveCooldown?.RecordUse();
        }, forceEvenIfRemoved: true);
    }
}
