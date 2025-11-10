#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

[ExecuteInEditMode]
public class SnapToGrid : MonoBehaviour
{
#if UNITY_EDITOR
    [SerializeField] private float textureXSize = 0.9f;
    [SerializeField] private float textureYSize = 0.9f;
    [SerializeField] private int snapDivX = 2;
    [SerializeField] private int snapDivY = 2;
    [SerializeField] private Vector3 offset;
    private Transform parent;
    void Start()
    {
        parent = transform;
    }
    void Update()
    {
        if (!UnityEditor.EditorApplication.isPlaying)
        {
            Vector3 position = parent.position - offset;
            textureXSize /= snapDivX;
            textureYSize /= snapDivY;
            position = new Vector3(
                Mathf.Round(position.x / textureXSize) * textureXSize,
                Mathf.Round(position.y / textureYSize) * textureYSize,
                Mathf.Round(position.z / textureXSize) * textureXSize
            );
            textureXSize *= snapDivX;
            textureYSize *= snapDivY;
            parent.position = position + offset;
        }
    }
#endif
}
