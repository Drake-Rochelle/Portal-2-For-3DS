#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

[ExecuteInEditMode]
public class WallsManager : MonoBehaviour
{
#if UNITY_EDITOR
    [SerializeField] private MeshRenderer px;
    [SerializeField] private Material[] wall;
    [SerializeField] private float textureXSize;
    [SerializeField] private float textureYSize;
    [SerializeField] private int layer;
    [SerializeField] private float layerOffset;
    [SerializeField] private int snapDivX = 2;
    [SerializeField] private int snapDivY = 2;
    [SerializeField] private bool update;
    [SerializeField] private bool floor;
    private Transform parent;
    void Start()
    {
        parent = transform;
    }
    void Update()
    {
        if (!UnityEditor.EditorApplication.isPlaying)
        {
            px.material = wall[0];
            Vector3 scale = new Vector3(parent.localScale.x - (layer * layerOffset), parent.localScale.y - (layer * layerOffset), parent.localScale.z - (layer * layerOffset));
            scale = new Vector3(
                Mathf.Max(Mathf.Round(scale.x / (textureXSize / snapDivX)) * (textureXSize / snapDivX), (textureXSize / snapDivX)) + (layer * layerOffset),
                Mathf.Max(Mathf.Round(scale.y / (textureYSize / snapDivY)) * (textureYSize / snapDivY), (textureYSize / snapDivY)) + (layer * layerOffset),
                Mathf.Max(Mathf.Round(scale.z / (textureXSize / snapDivX)) * (textureXSize / snapDivX), (textureXSize / snapDivX)) + (layer * layerOffset)
            );
            parent.localScale = scale;
            scale = new Vector3(scale.x - (layer * layerOffset), scale.y - (layer * layerOffset), scale.z - (layer * layerOffset));
            Vector3 position = parent.position;
            textureXSize /= snapDivX;
            textureYSize /= snapDivY;
            position = new Vector3(
                Mathf.Round(position.x / textureXSize) * textureXSize,
                Mathf.Round(position.y / textureYSize) * textureYSize,
                Mathf.Round(position.z / textureXSize) * textureXSize
            );
            textureXSize *= snapDivX;
            textureYSize *= snapDivY;
            parent.position = position;
            Vector2 tileYZ = new Vector2(scale.z / textureXSize, scale.y / textureYSize);
            Vector2 tileXZ = new Vector2(scale.z / (textureXSize), scale.x / (textureXSize));
            if (!floor)
            {
                px.material.mainTextureScale = tileYZ;
            }
            else
            {
                px.material.mainTextureScale = tileXZ;
            }
        }
    }
#endif
}
