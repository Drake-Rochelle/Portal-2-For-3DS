using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TransitionManager : MonoBehaviour
{
    [SerializeField] private Vector3 fadeInHoldFadeOut;
    [SerializeField] private Image fade;
    public static TransitionManager Instance
    {
        get; private set;
    }
    private void Start()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        DontDestroyOnLoad(gameObject);
        StartCoroutine(FadeIn());
    }
    public void Scene(string name)
    {
        StartCoroutine(LoadScene(name));
    }
    private IEnumerator FadeIn()
    {
        float timer = 0;
        while (timer < fadeInHoldFadeOut.z)
        {
            timer += Time.unscaledDeltaTime;
            fade.color = new Color(fade.color.r, fade.color.g, fade.color.b, 1 - (timer / fadeInHoldFadeOut.z));
            yield return null;
        }
    }
    private IEnumerator LoadScene(string name)
    {
        float timer = 0;
        while (timer < fadeInHoldFadeOut.x)
        {
            timer += Time.unscaledDeltaTime;
            fade.color = new Color(fade.color.r, fade.color.g, fade.color.b,timer/fadeInHoldFadeOut.x);
            yield return null;
        }
        SceneManager.LoadScene(name);
        while (SceneManager.GetActiveScene().name != name)
        {
            yield return null;
        }
        timer = 0;
        while (timer < fadeInHoldFadeOut.y)
        {
            timer += Time.unscaledDeltaTime;
            fade.color = new Color(fade.color.r, fade.color.g, fade.color.b, 1);
            yield return null;
        }
        timer = 0;
        while (timer<fadeInHoldFadeOut.z)
        {
            timer += Time.unscaledDeltaTime;
            fade.color = new Color(fade.color.r, fade.color.g, fade.color.b, 1-(timer / fadeInHoldFadeOut.z));
            yield return null;
        }
    }
}
