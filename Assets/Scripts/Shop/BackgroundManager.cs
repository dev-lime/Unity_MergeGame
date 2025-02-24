using System;
using System.Collections;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    public UIThemeManager themeManager;
    public Sprite[] backgrounds;
    public float fadeDuration = 0.5f;

    private SpriteRenderer backgroundRenderer;

    private void Start()
    {
        backgroundRenderer = GetComponent<SpriteRenderer>();
        themeManager.UpdateUIColors(GetAverageBackgroundColor(backgroundRenderer), fadeDuration);
    }

    public void SetBackgroundById(int id = 0)
    {
        if (backgrounds.Length == 0 || backgroundRenderer == null)
        {
            Debug.LogWarning("Array backgrounds is empty or background renderer is null");
            return;
        }

        // Проверяем, чтобы индекс был в пределах массива
        if (id >= 0 && id < backgrounds.Length)
        {
            StartCoroutine(FadeOutAndIn(id));
        }
        else
        {
            Debug.LogWarning("Индекс выбранного элемента выходит за пределы массива фонов!");
        }
    }

    private IEnumerator FadeOutAndIn(int index)
    {
        // Создаём временный спрайт позади текущего
        GameObject newSpriteObject = new GameObject("TempSprite");
        SpriteRenderer newSpriteRenderer = newSpriteObject.AddComponent<SpriteRenderer>();

        newSpriteRenderer.sprite = backgrounds[index];
        newSpriteRenderer.transform.position = backgroundRenderer.transform.position;
        newSpriteRenderer.transform.localScale = backgroundRenderer.transform.localScale;
        newSpriteRenderer.sortingOrder = backgroundRenderer.sortingOrder - 1; // Поместить за основным

        themeManager.UpdateUIColors(GetAverageBackgroundColor(newSpriteRenderer), fadeDuration);

        // Плавное исчезновение старого спрайта
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            float alpha = 1 - (t / fadeDuration);
            backgroundRenderer.color = new Color(1, 1, 1, alpha);
            yield return null;
        }

        // Полностью заменяем основной спрайт
        backgroundRenderer.sprite = backgrounds[index];
        backgroundRenderer.color = Color.white; // Возвращаем прозрачность
        Destroy(newSpriteObject); // Удаляем временный объект
    }

    Color GetAverageBackgroundColor(SpriteRenderer renderer)
    {
        Texture2D texture = SpriteToTexture(renderer.sprite);
        if (texture == null) return Color.black;

        Color[] pixels = texture.GetPixels();
        if (pixels.Length == 0) return Color.black;

        float r = 0, g = 0, b = 0, a = 0;
        foreach (Color color in pixels)
        {
            r += color.r;
            g += color.g;
            b += color.b;
            a += color.a;
        }

        int count = pixels.Length;
        return new Color(r / count, g / count, b / count, a / count);
    }

    Texture2D SpriteToTexture(Sprite sprite)
    {
        if (sprite == null) return null;

        // Создаём новую текстуру из спрайта
        RenderTexture rt = new RenderTexture(sprite.texture.width, sprite.texture.height, 0);
        Graphics.Blit(sprite.texture, rt);

        Texture2D newTexture = new Texture2D(rt.width, rt.height, TextureFormat.RGBA32, false);
        RenderTexture.active = rt;
        newTexture.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        newTexture.Apply();
        RenderTexture.active = null;
        rt.Release();

        return newTexture;
    }
}
