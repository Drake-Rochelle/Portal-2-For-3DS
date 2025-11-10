using UnityEngine;

public class ButtonManager : MonoBehaviour 
{
    [HideInInspector] public bool isPressed;
    [HideInInspector] public bool isDown;
    [SerializeField] private Vector3 pressDistance;
	[SerializeField] private float speed;
	private Transform child;
	private bool down;
	private Vector3 delta;
	private Vector3 init;
	private void Start()
	{
		delta = pressDistance*speed;
		child = transform.GetChild(0).GetChild(0);
        init = child.position;
    }
    public void Press()
	{
		down = true;
		isPressed = true;
	}
	public void Release()
	{
		isPressed = false;
	}
	void Update()
	{
		if (down)
		{
			child.Translate(transform.rotation * (delta * Time.deltaTime), Space.World);
		}
		else
		{
			child.Translate(transform.rotation * (-delta * Time.deltaTime), Space.World);
        }
		if (child.position.y < init.y + pressDistance.y)
		{
            child.position = init+(transform.rotation * pressDistance);
		}
		else if (child.position.y > init.y)
		{
            child.position = init;
		}
		if (child.position == init + (transform.rotation * pressDistance) && !isPressed)
		{
			down = false;
		}
		isDown = false;
		if (child.position != init)
		{
			isDown = true;
		}
    }
}
