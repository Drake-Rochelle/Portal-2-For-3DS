using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Bootloader : MonoBehaviour 
{
    [SerializeField] private string[] scenes;
    [SerializeField] private string[] gameScenes;
    [SerializeField] private Sprite[] loadingScreens;
    private int state;
    public static Bootloader Instance
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
    public void NextScene()
    {
        if (gameScenes.Contains<string>(scenes[state]))
        {
            TransitionManager.Instance.Scene(scenes[state], loadingScreens[Random.Range(0,loadingScreens.Length)]);
        }
        else
        {
            TransitionManager.Instance.Scene(scenes[state]);
        }
        state++;
        state %= scenes.Length;
    }
    public void SetState(string name)
    {
        state = scenes.ToList().IndexOf(name);
        NextScene();
    }
}
