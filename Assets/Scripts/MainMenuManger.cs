using UnityEngine;
using UnityEngine.UI;
public class MainMenuManger : MonoBehaviour 
{
	[SerializeField] private RawImage image;
	[SerializeField] private Texture2D[] menuImages;
    [SerializeField] private AudioClip buttonClip;
    [SerializeField] private AudioClip noClip;
    [SerializeField] private AudioClip pressClip;
    [SerializeField] private AudioSource audioSource;
	private int button;
	private bool update;
	void Update () 
	{
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
                audioSource.PlayOneShot(pressClip);
                Bootloader.Instance.NextScene();
            }
			else if (button == 3)
			{
                audioSource.PlayOneShot(pressClip);
                TransitionManager.Instance.Scene("Settings");
            }
            else
			{
                audioSource.PlayOneShot(noClip);
            }
        }
		if (update)
		{
			audioSource.PlayOneShot(buttonClip);
			image.texture = menuImages[button];
		}
    }
}
