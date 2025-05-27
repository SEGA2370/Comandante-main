using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] GameObject glow;
    [SerializeField] GameObject nextLevel;
    [SerializeField] GameObject option;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            glow.gameObject.SetActive(true);
            nextLevel.gameObject.SetActive(true);
            Invoke("ActivateOption", 2f);
        }
    }
    private void ActivateOption()
    {
        option.gameObject.SetActive(true);
    }
}
