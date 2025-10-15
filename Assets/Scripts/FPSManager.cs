using UnityEngine;

public class FPSManager : MonoBehaviour 
{
	[SerializeField] private TextMesh text;

	void Update () 
	{
		text.text = "FPS: "+((int)(1 / Time.smoothDeltaTime)).ToString();
	}
}
