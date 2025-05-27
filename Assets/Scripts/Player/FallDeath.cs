using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class FallDeath : MonoBehaviour
{
    [Header("Fall settings")]
    [SerializeField] private float fallThreshold = -10f;

    [Header("Fade settings")]
    [SerializeField] private Image fadeImage;        // full-screen black Image, alpha = 0 at start
    [SerializeField] private float fadeDuration = 1f;

    private bool isFading = false;

    void Update()
    {
        if (!isFading && transform.position.y < fallThreshold)
            StartCoroutine(FadeAndReload());
    }

    private IEnumerator FadeAndReload()
    {
        isFading = true;

        // grab the image's original color (so we preserve its R/G/B)
        Color c = fadeImage.color;
        c.a = 0f;                // start fully transparent
        fadeImage.color = c;

        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            // calculate alpha from 0→1
            c.a = Mathf.Clamp01(timer / fadeDuration);
            // only change the alpha, leave c.r/g/b untouched
            fadeImage.color = c;
            yield return null;
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
