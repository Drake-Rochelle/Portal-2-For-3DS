using UnityEngine;

public class SceneManager : MonoBehaviour 
{
	[SerializeField] private GameObject[] scenes;
    [SerializeField] private Transform dontDestroyOnLoad;
    [SerializeField] private Transform doDestroyOnLoad;
    //==============================SINGLETON==============================
    public static SceneManager Instance
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
    public void LoadScene(string sceneName)
    {
        for(int i = 0; i < scenes.Length; i++)
        {
            if (scenes[i].name == sceneName)
            {
                if (doDestroyOnLoad.childCount > 0)
                {
                    Destroy(doDestroyOnLoad.GetChild(0).gameObject);
                }
                Instantiate(scenes[i], doDestroyOnLoad);
            }
        }
    }
    public void DontDestroy(Transform go)
    {
        go.parent = dontDestroyOnLoad;
    }
    public string GetActiveScene()
    {
        return doDestroyOnLoad.GetChild(0).name.Replace("(Clone)","");
    }
}
