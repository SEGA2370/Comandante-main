using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchInputManager : MonoBehaviour
{
    public static TouchInputManager Instance { get; private set; }

    // state
    public bool LeftHeld { get; private set; }
    public bool RightHeld { get; private set; }
    public bool JumpHeld { get; private set; }
    public bool FireHeld { get; private set; }

    // horizontal axis: -1 = left, +1 = right, 0 = neutral
    public float Horizontal => RightHeld ? 1f : LeftHeld ? -1f : 0f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    // called by the button
    public void SetButtonState(string buttonName, bool isDown)
    {
        switch (buttonName)
        {
            case "Left": LeftHeld = isDown; break;
            case "Right": RightHeld = isDown; break;
            case "Jump": JumpHeld = isDown; break;
            case "Fire": FireHeld = isDown; break;
        }
    }
}