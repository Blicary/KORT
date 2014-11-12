using UnityEngine;
using System.Collections;

public class ScreenFadeInOut : MonoBehaviour
{
    public float default_fade_seconds = 1.5f;
    private float fade_seconds = 1.5f;
    private float current_fade_time = 0;


    public void Awake()
    {
        // Set the texture so that it is the the size of the screen and covers it.
        guiTexture.pixelInset = new Rect(0f, 0f, Screen.width, Screen.height);
    }

    private IEnumerator UpdateFadeBlack()
    {
        while (true)
        {
            current_fade_time += Time.unscaledDeltaTime;

            // Lerp the colour of the texture between itself and black.
            guiTexture.color = Color.Lerp(guiTexture.color, Color.black, current_fade_time / fade_seconds);
            
            // If the screen is almost black...
            if (current_fade_time >= fade_seconds) break;

            yield return null;
        }
    }
    private IEnumerator UpdateFadeClear()
    {
        while (true)
        {
            current_fade_time += Time.unscaledDeltaTime;

            // Lerp the colour of the texture between itself and transparent.
            guiTexture.color = Color.Lerp(guiTexture.color, Color.clear, current_fade_time / fade_seconds);

            // If the screen is almost clear...
            if (current_fade_time >= fade_seconds)
            {
                guiTexture.enabled = false;
                break;
            }

            yield return null;
        }
    }

    public void FadeToClear(float seconds)
    {
        fade_seconds = seconds;
        current_fade_time = 0;
        StopAllCoroutines();
        StartCoroutine("UpdateFadeClear");
    }
    public void FadeToClear()
    {
        FadeToClear(default_fade_seconds);
    }
    public void FadeToBlack(float seconds)
    {
        fade_seconds = seconds;
        current_fade_time = 0;
        guiTexture.enabled = true;
        StopAllCoroutines();
        StartCoroutine("UpdateFadeBlack");
    }
    public void FadeToBlack()
    {
        FadeToBlack(default_fade_seconds);
    }

    public void InstantBlack()
    {
        guiTexture.enabled = true;
        guiTexture.color = Color.black;
        StopAllCoroutines();
    }
    public void InstantClear()
    {
        guiTexture.enabled = false;
        guiTexture.color = Color.clear;
        StopAllCoroutines();
    }
}