using UnityEngine;

[ExecuteInEditMode]
public class WallsManager : MonoBehaviour
{
#if UNITY_EDITOR
    [SerializeField] private MeshRenderer px;
    [SerializeField] private MeshRenderer mx;
    [SerializeField] private MeshRenderer py;
    [SerializeField] private MeshRenderer my;
    [SerializeField] private MeshRenderer pz;
    [SerializeField] private MeshRenderer mz;
    [SerializeField] private Material[] wall;
    [SerializeField] private float textureXSize;
    [SerializeField] private float textureYSize;
    [SerializeField] private int layer;
    [SerializeField] private float layerOffset;
    [SerializeField] private int snapDivX = 2;
    [SerializeField] private int snapDivY = 2;
    [SerializeField] private bool update;
    private Transform parent;
    void Start()
    {
        parent = transform;
    }
    void Update()
    {
        if (update)
        {
            px.material = wall[0];
            mx.material = wall[1];
            py.material = wall[2];
            my.material = wall[3];
            pz.material = wall[4];
            mz.material = wall[5];
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
            Vector2 tileXY = new Vector2(scale.x / textureXSize, scale.y / textureYSize);
            Vector2 tileYZ = new Vector2(scale.z / textureXSize, scale.y / textureYSize);
            Vector2 tileXZ = new Vector2(scale.x / (textureXSize), scale.z / (textureXSize));
            px.material.mainTextureScale = tileYZ;
            mx.material.mainTextureScale = tileYZ;
            py.material.mainTextureScale = tileXZ;
            my.material.mainTextureScale = tileXZ;
            pz.material.mainTextureScale = tileXY;
            mz.material.mainTextureScale = tileXY;
        }
    }
#endif
}
