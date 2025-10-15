using UnityEngine;

public class SoundTrigger : TriggerPrimitive
{
    [SerializeField] private AudioClip audioClip;
    [SerializeField] private bool is2D;
    [SerializeField] private float volume;
    [SerializeField] private bool loop;
    private AudioSource source;
    private void Awake()
    {
        source = gameObject.AddComponent<AudioSource>();
        source.volume = volume;
        source.loop = loop;
        if (is2D)
        {
            source.spatialBlend = 0;
        }
        else
        {
            source.spatialBlend = 1;
        }
    }
    public override void Enter()
    {
        source.PlayOneShot(audioClip);
    }
}
