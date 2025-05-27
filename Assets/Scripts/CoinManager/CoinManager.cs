using TMPro;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI coinText;
    [SerializeField] TextMeshProUGUI totalCoinsText;
    private int coinCounter;
    private static int totalCoinsCount;

    private const string TotalCoinsKey = "TotalCoins";

    public int CoinCount
    {
        get { return coinCounter; }
        private set
        {
            coinCounter = value;
            UpdateCoinText();
        }
    }

    void Start()
    {
        LoadTotalCoins(); 
        UpdateCoinText();
        UpdateTotalCoinsText();
    }

    private void UpdateCoinText()
    {
        coinText.text = ":" + CoinCount.ToString();
    }

    private void UpdateTotalCoinsText()
    {
        if (totalCoinsText != null)
        {
            totalCoinsText.text = " " + totalCoinsCount.ToString();
        }
    }

    public void IncrementCoinCount()
    {
        CoinCount++;
        UpdateCoinText();
        totalCoinsCount++; 
        UpdateTotalCoinsText();
        SaveTotalCoins();
    }

    private void LoadTotalCoins()
    {
        if (PlayerPrefs.HasKey(TotalCoinsKey))
        {
            totalCoinsCount = PlayerPrefs.GetInt(TotalCoinsKey);
        }
    }

    private void SaveTotalCoins()
    {
        PlayerPrefs.SetInt(TotalCoinsKey, totalCoinsCount);
        PlayerPrefs.Save();
    }
}
