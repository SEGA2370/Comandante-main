using UnityEngine;
using UnityEngine.EventSystems; // дл€ проверки на тап по UI

public class InstantiatePointEffector : MonoBehaviour
{
    public GameObject pointEffectorPrefab;
    private GameObject currentEffectorInstance;

    [Tooltip("≈сли true Ч игнорируем тапы/клики поверх UI.")]
    [SerializeField] bool ignoreUI = true;

    Camera cam;

    void Awake()
    {
        cam = Camera.main; // можно задать вручную в инспекторе, если нужно
    }

    void Update()
    {
        // 1) ANDROID / ћќЅ»Ћ№Ќџ≈: первый тач (Phase Began)
        if (Input.touchCount > 0)
        {
            Touch t = Input.GetTouch(0);
            if (t.phase == TouchPhase.Began)
            {
                if (ignoreUI && EventSystem.current != null &&
                    EventSystem.current.IsPointerOverGameObject(t.fingerId))
                    return;

                Vector3 worldPos = ScreenToWorld(t.position);
                SpawnEffectorAt(worldPos);
                return;
            }
        }

        // 2) ѕ  / –≈ƒј “ќ–: клик мышью
        if (Input.GetMouseButtonDown(0))
        {
            if (ignoreUI && EventSystem.current != null &&
                EventSystem.current.IsPointerOverGameObject()) // дл€ мыши fingerId не нужен
                return;

            Vector3 worldPos = ScreenToWorld(Input.mousePosition);
            SpawnEffectorAt(worldPos);
        }
    }

    Vector3 ScreenToWorld(Vector3 screenPos)
    {
        if (cam == null) cam = Camera.main;
        Vector3 pos = cam.ScreenToWorldPoint(screenPos);
        pos.z = 0f; // 2D-плоскость
        return pos;
    }

    void SpawnEffectorAt(Vector3 worldPos)
    {
        if (pointEffectorPrefab == null) return;

        if (currentEffectorInstance != null)
            Destroy(currentEffectorInstance);

        currentEffectorInstance = Instantiate(pointEffectorPrefab, worldPos, Quaternion.identity);
    }
}
