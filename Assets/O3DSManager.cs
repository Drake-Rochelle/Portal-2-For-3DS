using UnityEngine;

public class O3DSManager : MonoBehaviour 
{
    [SerializeField] private Shader simple;
    [SerializeField] private Shader unlit;
    public bool N3DSMode = true;
    void Start()
    {
        if (!N3DSMode)
        {
            if (simple == null || unlit == null)
            {
                Debug.LogError("Shaders null.");
                return;
            }
            Renderer[] allRenderers = FindObjectsOfType<Renderer>();
            foreach (Renderer renderer in allRenderers)
            {
                foreach (Material mat in renderer.sharedMaterials)
                {
                    if (mat != null && mat.shader == simple)
                    {
                        mat.shader = unlit;
                    }
                }
            }
        }
    }
}
