using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ReviveCooldown : MonoBehaviour
{
    // How long to wait between revives
    [SerializeField] public int cooldownSeconds = 60;

    // UI
    [SerializeField] Button reviveButton;                 // your Revive button
    [SerializeField] TextMeshProUGUI cooldownTextTMP;     // optional TMP text near/on the button
    [SerializeField] Text cooldownText;                   // optional legacy Text
    [SerializeField] bool hideButtonWhileCooling = false; // or just disable

    const string KEY = "revive_last_ts"; // unix seconds
    Coroutine tickCo;

    void OnEnable()
    {
        UpdateState();
        tickCo ??= StartCoroutine(Tick());
    }

    void OnDisable()
    {
        if (tickCo != null) StopCoroutine(tickCo);
        tickCo = null;
    }

    IEnumerator Tick()
    {
        while (true)
        {
            UpdateState();
            yield return new WaitForSecondsRealtime(1f); // unaffected by Time.timeScale
        }
    }

    public bool CanUse() => RemainingSeconds() <= 0;

    public int RemainingSeconds()
    {
        long last = 0;
        if (long.TryParse(PlayerPrefs.GetString(KEY, "0"), out var v)) last = v;
        long now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        long rem = (last + cooldownSeconds) - now;
        return (int)Mathf.Max(0, rem);
    }

    public void RecordUse()
    {
        long now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        PlayerPrefs.SetString(KEY, now.ToString());
        PlayerPrefs.Save();
        UpdateState();
    }

    void UpdateState()
    {
        int rem = RemainingSeconds();
        bool ready = rem <= 0;

        if (reviveButton)
        {
            reviveButton.interactable = ready;
            if (hideButtonWhileCooling)
                reviveButton.gameObject.SetActive(ready);
        }

        string label = ready ? "Revive" : $"Revive in {rem}s";
        if (cooldownTextTMP) cooldownTextTMP.text = label;
        if (cooldownText) cooldownText.text = label;
    }
}
