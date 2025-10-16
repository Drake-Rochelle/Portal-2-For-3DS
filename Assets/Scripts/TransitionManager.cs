using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TransitionManager : MonoBehaviour
{
    [SerializeField] private Vector3 fadeInHoldFadeOut;
    [SerializeField] private Image fade;
    [SerializeField] private Image image;
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
        SceneManager.Instance.DontDestroy(transform);
        StartCoroutine(FadeIn());
    }
    public void Scene(string name, Sprite image = null)
    {
        if (image == null)
        {
            StartCoroutine(LoadScene(name));
        }
        else
        {
            StartCoroutine(LoadSceneImage(name, image));
        }
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
            fade.color = new Color(fade.color.r, fade.color.g, fade.color.b, timer / fadeInHoldFadeOut.x);
            yield return null;
        }
        SceneManager.Instance.LoadScene(name);
        while (SceneManager.Instance.GetActiveScene() != name)
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
        while (timer < fadeInHoldFadeOut.z)
        {
            timer += Time.unscaledDeltaTime;
            fade.color = new Color(fade.color.r, fade.color.g, fade.color.b, 1 - (timer / fadeInHoldFadeOut.z));
            yield return null;
        }
    }
    private IEnumerator LoadSceneImage(string name, Sprite imageSprite)
    {
        float timer = 0;
        while (timer < fadeInHoldFadeOut.x)
        {
            timer += Time.unscaledDeltaTime;
            fade.color = new Color(fade.color.r, fade.color.g, fade.color.b, timer / fadeInHoldFadeOut.x);
            yield return null;
        }


        image.sprite = imageSprite;
        image.color = new Color(255, 255, 255, 255);


        timer = 0;
        while (timer < fadeInHoldFadeOut.x)
        {
            timer += Time.unscaledDeltaTime;
            fade.color = new Color(fade.color.r, fade.color.g, fade.color.b, 1 - (timer / fadeInHoldFadeOut.x));
            yield return null;
        }

        SceneManager.Instance.LoadScene(name);
        while (SceneManager.Instance.GetActiveScene() != name)
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
        while (timer < fadeInHoldFadeOut.x)
        {
            timer += Time.unscaledDeltaTime;
            fade.color = new Color(fade.color.r, fade.color.g, fade.color.b, timer / fadeInHoldFadeOut.x);
            yield return null;
        }


        image.color = new Color(255, 255, 255, 0);


        timer = 0;
        while (timer < fadeInHoldFadeOut.x)
        {
            timer += Time.unscaledDeltaTime;
            fade.color = new Color(fade.color.r, fade.color.g, fade.color.b, 1 - (timer / fadeInHoldFadeOut.x));
            yield return null;
        }
    }
}
