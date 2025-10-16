using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class LightingBakingPrepper : EditorScript
{
#if UNITY_EDITOR
    [SerializeField] private bool bake;
    private bool prevBake;
    private List<GameObject> noShadowCasters;
    private bool moved;
    private List<Transform> objTransforms;
    void Awake()
    {
        objTransforms = new List<Transform>();
    }
    void Update()
    {
        Debug.Log(prevBake);
        Debug.Log(bake);
        if (bake && !prevBake)
        {
            OnBakeStart();
        }
        prevBake = bake;
        if (!bake && objTransforms.Count!=0)
        {
            for (int i = 0; i < noShadowCasters.Count; i++)
            {
                noShadowCasters[i].transform.position = objTransforms[i].position;
            }
            objTransforms.Clear();
        }
    }
    private void OnBakeStart()
    {
        moved = true;
        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        List<GameObject> noShadowCasters = new List<GameObject>();
        Debug.Log(allObjects.Length);
        foreach (GameObject obj in allObjects)
        {
            if (!obj.isStatic)
            {
                if (obj.transform.parent == null)
                {
                    Debug.Log(obj.name);
                    noShadowCasters.Add(obj);
                    objTransforms.Add(obj.transform);
                }
                else
                {
                    if (obj.transform.parent.gameObject.isStatic)
                    {
                        Debug.Log(obj.name);
                        noShadowCasters.Add(obj);
                        objTransforms.Add(obj.transform);
                    }
                }
            }
        }
        for (int i = 0; i < noShadowCasters.Count; i++)
        {
            noShadowCasters[i].transform.position = new Vector3(
                noShadowCasters[i].transform.position.x * 99,
                noShadowCasters[i].transform.position.y * 99,
                noShadowCasters[i].transform.position.z * 99
            );
        }
    }
    private void OnBakeFinish()
    {
        if (moved)
        {
            moved = false;
            for (int i = 0; i < noShadowCasters.Count; i++)
            {
                noShadowCasters[i].transform.position = new Vector3(
                    noShadowCasters[i].transform.position.x / 99,
                    noShadowCasters[i].transform.position.y / 99,
                    noShadowCasters[i].transform.position.z / 99
                );
            }
        }
    }
#endif
}
