using UnityEngine;

public class AudioManager : MonoBehaviour 
{
    [SerializeField] AudioSource audioSource;
    public static AudioManager Instance
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
    public void Play(AudioClip c, AudioSource a = null)
    {
        if(a == null)
        {
            audioSource.PlayOneShot(c);
        }
        else
        {
            a.PlayOneShot(c);
        }
    }
}
