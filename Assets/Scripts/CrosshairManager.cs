using UnityEngine;
using UnityEngine.UI;

public class CrosshairManager : MonoBehaviour 
{

	[SerializeField] private Sprite[] crosshairs;
	private Image mr;
	public bool update;
	public int crosshair;
    //==============================SINGLETON==============================
    public static CrosshairManager Instance
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
    //=====================================================================
    void Start()
	{
		mr = GetComponent<Image>();
	}

	void Update () 
	{
		if (update)
		{
			update = false;
			mr.sprite = crosshairs[crosshair];
		}
	}
}
