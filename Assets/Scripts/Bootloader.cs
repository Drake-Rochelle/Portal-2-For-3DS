using UnityEngine;

public class Bootloader : MonoBehaviour 
{
    [SerializeField] private string[] scenes;
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
        TransitionManager.Instance.Scene(scenes[state]);
        state++;
        state %= scenes.Length;
    }
}
