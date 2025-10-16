using UnityEngine;
using UnityEngine.UI;
public class PortalGunAnimator : MonoBehaviour 
{
	[SerializeField] private float intensity;
	[SerializeField] private float speed;
    [SerializeField] private Image image;
    [SerializeField] private Sprite[] sprites;
    [HideInInspector] public bool animate;
    [HideInInspector] public int color;
    [HideInInspector] public bool update;
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
        image.sprite = sprites[0];
    }
    void Start () 
	{
		t = GetComponent<RectTransform>();
        off = t.anchoredPosition;
	}
	void Update () 
	{
        if (update)
        {
            update = false;
            image.sprite = sprites[color+1];
        }
        if (animate)
        {
            t.anchoredPosition = new Vector2(Mathf.Sin(Time.time * speed) * intensity, 0)+off;
        }
	}
}
