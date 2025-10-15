using UnityEngine;

public class TouchTest : MonoBehaviour 
{
	void Update () 
	{
		transform.position = new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, transform.position.z);
	}
}
