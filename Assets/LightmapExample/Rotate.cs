using UnityEngine;
using System.Collections;

public class Rotate : MonoBehaviour
{
	void Start()
	{
		Vector3 pos = transform.position;
		distance = pos.magnitude;
		direction = Mathf.Atan2(pos.z, pos.x);
		elevation = Mathf.Asin(pos.y / distance);
	}

	void Update()
	{
		direction += 0.01f;
		float rCosE = distance * Mathf.Cos(elevation);
		transform.position = new Vector3(rCosE * Mathf.Cos(direction), distance * Mathf.Sin(elevation), rCosE * Mathf.Sin(direction));
		transform.LookAt(Vector3.zero);
	}

	private float direction;
	private float elevation;
	private float distance;
}
