using UnityEngine;
using System.Collections;

public class Spin : MonoBehaviour
{
	void Update()
	{
		transform.Rotate(0.0f, Time.deltaTime * 200.0f, 0.0f);
	}
}
