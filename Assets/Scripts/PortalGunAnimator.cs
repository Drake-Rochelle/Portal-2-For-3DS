using UnityEngine;
using UnityEngine.UI;
public class PortalGunAnimator : MonoBehaviour 
{
	[SerializeField] private float intensity;
	[SerializeField] private float speed;
    [SerializeField] private Sprite[] sprites;
    [HideInInspector] public bool animate;
    [HideInInspector] public int color;
    private RectTransform t;
    private Vector2 off;
    private Image image;
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
        image = GetComponent<Image>();
    }
    void Start () 
	{
		t = GetComponent<RectTransform>();
        off = t.anchoredPosition;
	}
	void Update () 
	{
        if (sprites[color] != image.sprite)
        {
            image.sprite = sprites[color];
        }
        if (animate)
        {
            t.anchoredPosition = new Vector2(Mathf.Sin(Time.time * speed) * intensity, 0)+off;
        }
	}
}
