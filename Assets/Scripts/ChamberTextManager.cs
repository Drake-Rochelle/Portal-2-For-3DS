using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChamberTextManager : MonoBehaviour 
{
	private Text text;
	void Start () 
	{
		text = GetComponent<Text>();
		string sceneName = SceneManager.GetActiveScene().name;
		sceneName = sceneName.Split('_')[0];
		sceneName = sceneName.Split('r')[1];
		if (sceneName.Length == 1)
		{
			text.text = "Chamber 0" + sceneName;
		}
		else
		{
			text.text = "Chamber " + sceneName;
		}
    }
}
