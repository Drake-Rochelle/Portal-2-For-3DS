using System.IO;
using UnityEngine;
using UnityEngine.UI;
public class MainMenuManger : MonoBehaviour 
{
	[SerializeField] private RawImage image;
	[SerializeField] private Texture2D[] menuImages;
    [SerializeField] private AudioClip buttonClip;
    [SerializeField] private AudioClip noClip;
    [SerializeField] private AudioClip pressClip;
    [SerializeField] private AudioClip[] music;
    [SerializeField] private int touchScreenTick;
    [SerializeField] private string[] items;
    [SerializeField] private string[] chambers;
    private int button;
	private bool update;
    private float position;
    private float prev;
    private bool tick;
    private bool prevPress;
    private bool press;
    void Start()
    {
        AudioManager.Instance.Play(transform, music[Random.Range(0, music.Length)]);
    }
	void Update () 
	{
        update = false;
#if !UNITY_EDITOR
        if (Input.touchCount!=0&&!prevPress)
        {
            tick = false;
        }
        if (Input.touchCount==0&&prevPress)
        {
            if (!tick)
            {
                press = true;
            }
        }
        if (Input.touchCount!=0)
        {
            prev = position;
            position += Input.GetTouch(0).deltaPosition.y;
            update = ((int)(prev / touchScreenTick) != (int)(position / touchScreenTick));
        }
#else
        if (Input.GetMouseButton(0) && !prevPress)
        {
            tick = false;
        }
        if (!Input.GetMouseButton(0) && prevPress)
        {
            if (!tick)
            {
                press = true;
            }
        }
        if (Input.GetMouseButton(0))
        {
            prev = position;
            position += Input.GetAxis("Mouse Y") * 3;
            update = ((int)(prev / touchScreenTick) != (int)(position / touchScreenTick));
        }
#endif
        if (Input.GetKeyDown(KeyCode.Keypad2) || Input.GetKeyDown(KeyCode.DownArrow))

        {
			button++;
			update = true;
		}
		else if (Input.GetKeyDown(KeyCode.Keypad8) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            button--;
            update = true;
        }
        int selection = (int)(position / touchScreenTick) + button;
        while (selection < 0)
        {
            selection += menuImages.Length;
        }
        while (selection > menuImages.Length-1)
        {
            selection -= menuImages.Length;
        }
        if (Input.GetKeyDown(KeyCode.A)||press)
		{
            press = false;
            if (selection == 0)
			{
                AudioManager.Instance.Play(transform, pressClip);
                string path = Path.Combine(Application.persistentDataPath, "CHAMBER");
                int index = 0;
                if (File.Exists(path))
                {
                    index = int.Parse(File.ReadAllText(path));
                }
                Bootloader.Instance.LoadChamber(index, true, 0);
            }
            else
			{
                AudioManager.Instance.Play(transform, noClip);
            }
        }
		if (update)
		{
            tick = true;
            AudioManager.Instance.Play(transform, buttonClip);
            image.texture = menuImages[selection];
            MainMenuBottomScreenManager.Instance.main.text = items[selection];
            if (selection == 0)
            {
                MainMenuBottomScreenManager.Instance.previous.text = items[items.Length-1];
            }
            else
            {
                MainMenuBottomScreenManager.Instance.previous.text = items[selection - 1];
            }
            if (selection == items.Length - 1)
            {
                MainMenuBottomScreenManager.Instance.next.text = items[0];
            }
            else
            {
                MainMenuBottomScreenManager.Instance.next.text = items[selection + 1];
            }
        }
#if !UNITY_EDITOR
        prevPress = Input.touchCount>0;
#else
        prevPress = Input.GetMouseButton(0);
#endif
    }
}
