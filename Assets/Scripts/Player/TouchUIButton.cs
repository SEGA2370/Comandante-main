using UnityEngine;
using UnityEngine.EventSystems;

public class TouchUIButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Tooltip("Must match one of: Left, Right, Jump, Fire")]
    public string buttonName;

    public void OnPointerDown(PointerEventData eventData)
        => TouchInputManager.Instance.SetButtonState(buttonName, true);

    public void OnPointerUp(PointerEventData eventData)
        => TouchInputManager.Instance.SetButtonState(buttonName, false);
}
