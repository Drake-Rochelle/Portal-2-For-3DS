using UnityEngine;
public class PortalGunAnimator : MonoBehaviour 
{
	[SerializeField] private float intensity;
	[SerializeField] private float speed;
    public bool animate;
    private RectTransform t;
    private Vector2 off;
    public static PortalGunAnimator Instance
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
    void Start () 
	{
		t = GetComponent<RectTransform>();
        off = t.anchoredPosition;
	}
	void Update () 
	{
        if (animate)
        {
            t.anchoredPosition = new Vector2(Mathf.Sin(Time.time * speed) * intensity, 0)+off;
        }
	}
}
