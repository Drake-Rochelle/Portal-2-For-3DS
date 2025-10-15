using UnityEngine;

public class WallsMetaManager : MonoBehaviour 
{
    public bool updateWallls;
    //==============================SINGLETON==============================
    public static WallsMetaManager Instance
    {
        get; private set;
    }
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("More than one " + this.name + ", ya chump");
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    //=====================================================================
}
