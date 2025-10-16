using UnityEngine;
[ExecuteInEditMode]
public class EditorScript : MonoBehaviour
{
#if UNITY_EDITOR
    void Start()
    {

    }
    public virtual void OnUpdate()
    {

    }
    void Update()
    {
        if (!UnityEditor.EditorApplication.isPlaying)
        {
            OnUpdate();
        }
    }
#endif
}
