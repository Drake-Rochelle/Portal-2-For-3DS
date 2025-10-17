using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    private void Update()
    {
#if !UNITY_EDITOR
        if (UnityEngine.N3DS.GamePad.GetButtonTrigger(N3dsButton.ZL) && UnityEngine.N3DS.GamePad.GetButtonHold(N3dsButton.ZR))
#else
		if (Input.GetKeyDown(KeyCode.F1))
#endif
        {
            Debug.Log("uibgr");
            SceneManager.LoadScene(scenes[state]);
            state++;
            state %= scenes.Length;
        }
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
        DontDestroyOnLoad(gameObject);
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
}
