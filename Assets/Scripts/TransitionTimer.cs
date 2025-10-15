using UnityEngine;

public class TransitionTimer : MonoBehaviour 
{
	[SerializeField] private float delay;
	private float start;
	private bool hasTransitioned;
	void Start()
	{
		start = Time.time;
	}
	void Update () 
	{
		if ((Time.time - start > delay) && !hasTransitioned)
		{
			hasTransitioned = true;
			Bootloader.Instance.NextScene();
		}
	}
}
