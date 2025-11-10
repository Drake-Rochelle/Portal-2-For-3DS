using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootloader : MonoBehaviour 
{
    [SerializeField] private string[] scenes;
    [SerializeField] private string[] gameScenes;
    [SerializeField] private string mainMenu;
    [SerializeField] private Sprite[] gameLoadingScreens;
    [SerializeField] private Sprite[] chamberLoadingScreens;
    [SerializeField] private Sprite[] menuLoadingScreens;
    [SerializeField] private Sprite black;
    public bool N3DSMode;
    private int state;
    private int currentChamber;
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
        N3DSMode = UnityEngine.N3DS.Application.isRunningOnSnake;
    }
    public void NextScene()
    {
        if (gameScenes.Contains<string>(scenes[state]))
        {
            Sprite image = gameLoadingScreens[Random.Range(0, gameLoadingScreens.Length)];
            TransitionManager.Instance.Scene(scenes[state], image, true, image);
        }
        else if (mainMenu == scenes[state])
        {
            Sprite image = menuLoadingScreens[Random.Range(0, menuLoadingScreens.Length)];
            TransitionManager.Instance.Scene(scenes[state], image, false, image);
        }
        else
        {
            TransitionManager.Instance.Scene(scenes[state]);
        }
        state++;
        state %= scenes.Length;
    }

    public void NextChmaber(int loadScreen)
    {
        LoadChamber(currentChamber+1, false, loadScreen);
    }
    public void LoadChamber(int index, bool fromMainMenu, int loadScreen)
    {
        if (index >= gameScenes.Length)
        {
            TransitionManager.Instance.Scene(mainMenu);
            File.WriteAllText(Path.Combine(Application.persistentDataPath, "CHAMBER"), "0");
        }
        else
        {
            if (fromMainMenu)
            {
                Sprite image = gameLoadingScreens[loadScreen];
                TransitionManager.Instance.Scene(gameScenes[index], image, fromMainMenu, image);
            }
            else
            {
                Sprite image = chamberLoadingScreens[loadScreen];
                TransitionManager.Instance.Scene(gameScenes[index], image, fromMainMenu, black);
            }
            currentChamber = index;
            File.WriteAllText(Path.Combine(Application.persistentDataPath, "CHAMBER"), currentChamber.ToString());
        }
    }
}
