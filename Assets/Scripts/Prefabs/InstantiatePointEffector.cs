using UnityEngine;

public class InstantiatePointEffector : MonoBehaviour
{
    public GameObject pointEffectorPrefab; 
    private GameObject currentEffectorInstance;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (currentEffectorInstance != null)
            {
                Destroy(currentEffectorInstance);
            }
            InstantiateEffector();
        }
    }

    public void InstantiateEffector()
    {
        if (pointEffectorPrefab != null)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0f;
            currentEffectorInstance = Instantiate(pointEffectorPrefab, mousePos, Quaternion.identity);
        }
    }
}
