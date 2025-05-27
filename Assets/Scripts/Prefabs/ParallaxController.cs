using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxController : MonoBehaviour
{
    [SerializeField] private Transform[] layers;
    [SerializeField] private float[] coefficient;

    private int LayerCount;

    private void Start()
    {
        LayerCount = layers.Length;
    }

    private void Update()
    {
        for (int i = 0; i < LayerCount; i++) 
        {
            layers[i].position = transform.position * coefficient[i];
        }
    }
}
