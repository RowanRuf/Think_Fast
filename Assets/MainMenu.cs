using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    public bool fadeOnStart = true;
    public float fadeDuration = 1;
    public Color fadeColor;
    public Renderer rend;
    public Slowmotion[] hands;
    public static MainMenu Instance;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        if (fadeOnStart)
            FadeIn();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FadeIn()
    {
        Fade(1, 0);
    }
    public void FadeOut()
    {
        Fade(0, 1);
    }

    public void StartGame()
    {
        FadeOut();
        StartCoroutine(nextLevel());
    }
    public void RestartLevel()
    {
        fadeDuration = 2f;
        Time.timeScale = 1.0f;
        FadeOut();
        hands[0].enabled = false;
        hands[1].enabled = false;
        StartCoroutine(sameLevel());
    }
    public void GoToMain()
    {
        SceneManager.LoadScene(0);
    }

    public void Fade(float alphaIn, float alphaOut)
    {
        StartCoroutine(FadeIE(alphaIn, alphaOut));
    }
    public IEnumerator FadeIE(float alphaIn, float alphaOut)
    {
        float timer = 0f;
        while(timer <= fadeDuration)
        {
            Color newColor = fadeColor;
            newColor.a = Mathf.Lerp(alphaIn, alphaOut, timer / fadeDuration);

            rend.material.SetColor("_BaseColor", newColor);

            timer += Time.deltaTime;
            yield return null;
        }
        Color newColor2 = fadeColor;
        newColor2.a = alphaOut;

        rend.material.SetColor("_BaseColor", newColor2);
    }
    IEnumerator nextLevel()
    {
        yield return new WaitForSeconds(fadeDuration);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    IEnumerator sameLevel()
    {
        yield return new WaitForSeconds(fadeDuration);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
