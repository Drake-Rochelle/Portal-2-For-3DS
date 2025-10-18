using System.Collections;
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
    [SerializeField] private AudioSource buttonAudioSource;
    [SerializeField] private AudioSource musicAudioSource;
    [SerializeField] private Text text;
    [SerializeField] private float showNotification;
    private bool N3DSMode;
    private int button;
	private bool update;
    private float timer = -1;
    void Start()
    {
        N3DSMode = UnityEngine.N3DS.Application.isRunningOnSnake;
        musicAudioSource.PlayOneShot(music[Random.Range(0, music.Length)]);
    }
	void Update () 
	{
        if (timer != -1)
        {
            timer += Time.deltaTime;
        }
        if (timer>showNotification)
        {
            timer = -1;
            text.text = "";
        }
        update = false;
        if (Input.GetKeyDown(KeyCode.Keypad2) || Input.GetKeyDown(KeyCode.DownArrow))

        {
			button++;
			if (button == menuImages.Length)
			{
				button = 0;
			}
			update = true;
		}
		else if (Input.GetKeyDown(KeyCode.Keypad8) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            button--;
            if (button == -1)
            {
                button = menuImages.Length-1;
            }
            update = true;
        }
		if (Input.GetKeyDown(KeyCode.A))
		{
            if (button == 0)
			{
                StartCoroutine(PlayAndDestroy(pressClip, buttonAudioSource.volume));
                Bootloader.Instance.NextScene();
            }
			else if (button == 3)
			{
                StartCoroutine(PlayAndDestroy(pressClip, buttonAudioSource.volume));
                Settings();
            }
            else
			{
                StartCoroutine(PlayAndDestroy(pressClip, buttonAudioSource.volume));
            }
        }
		if (update)
		{
            StartCoroutine(PlayAndDestroy(buttonClip, buttonAudioSource.volume));
            image.texture = menuImages[button];
        }
    }
    public IEnumerator PlayAndDestroy(AudioClip clip, float volume = 1f)
    {
        if (clip == null)
        {
            Debug.LogWarning("AudioRitual: Null clip passed to coroutine.");
            yield break;
        }
        GameObject tempGO = new GameObject("TempAudioSource");
        AudioSource aSource = tempGO.AddComponent<AudioSource>();
        aSource.clip = clip;
        aSource.volume = volume;
        aSource.playOnAwake = false;
        aSource.loop = false;
        aSource.Play();
        yield return new WaitForSeconds(clip.length);
        Destroy(tempGO);
    }
    void Settings()
    {
        N3DSMode = !N3DSMode;
        if (N3DSMode)
        {
            text.text = "Now in New 3DS Mode!";
        }
        else
        {
            text.text = "Now in Old 3DS Mode!";
        }
        Bootloader.Instance.N3DSMode = N3DSMode;
        timer = 0;
    }
}
