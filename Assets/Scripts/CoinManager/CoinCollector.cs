using UnityEngine;

public class CoinCollector : MonoBehaviour
{
    [SerializeField] CoinManager coinManager;

    private void OnTriggerEnter2D(Collider2D coin)
    {
        if (coin.gameObject.CompareTag("Coin"))
        {
            Destroy(coin.gameObject);
            coinManager.IncrementCoinCount();
        }
    }
}