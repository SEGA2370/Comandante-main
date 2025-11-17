using UnityEngine;

public class Parallax : MonoBehaviour
{
    [Range(0f, 0.5f)]
    public float speed = 0.1f;

    Renderer rend;
    MaterialPropertyBlock mpb;

    static readonly int MainTexST = Shader.PropertyToID("_MainTex_ST"); // Built-in
    static readonly int BaseMapST = Shader.PropertyToID("_BaseMap_ST"); // URP
    int activeST = 0;

    // Cached base ST from material (scale.xy, offset.xy)
    Vector4 baseST = new Vector4(1f, 1f, 0f, 0f);

    float offsetX;

    void Awake()
    {
        rend = GetComponent<Renderer>();
        mpb = new MaterialPropertyBlock();
    }

    void Start()
    {
        var mat = rend.sharedMaterial;
        if (mat != null)
        {
            if (mat.HasProperty(MainTexST)) activeST = MainTexST;
            else if (mat.HasProperty(BaseMapST)) activeST = BaseMapST;

            if (activeST != 0)
            {
                // Read initial ST from the shared material (no instancing)
                baseST = mat.GetVector(activeST);
                if (Mathf.Approximately(baseST.x, 0f)) baseST.x = 1f; // protect zero scale
                if (Mathf.Approximately(baseST.y, 0f)) baseST.y = 1f;
            }
        }

        if (activeST == 0)
            Debug.LogWarning($"{name}: shader has no _MainTex_ST/_BaseMap_ST; UV scroll won’t work.");
    }

    void Update()
    {
        if (activeST == 0) return;

        offsetX = (offsetX + speed * Time.deltaTime) % 1f;
        if (offsetX < 0f) offsetX += 1f;

        // Build ST with updated offset, keep original scale
        Vector4 st = baseST;
        st.z = (baseST.z + offsetX) % 1f; // offset.x
        // st.w stays as baseST.w (offset.y)

        rend.GetPropertyBlock(mpb);
        mpb.SetVector(activeST, st);
        rend.SetPropertyBlock(mpb);
    }
}
