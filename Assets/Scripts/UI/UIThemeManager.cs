using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIThemeManager : MonoBehaviour
{
    public Image[] uiElements; // UI элементы (кнопки, панели и т.п.)

    private float transitionDuration; // Длительность анимации
    private Color targetUIColor;

    public void UpdateUIColors(Color backgroundColor, float transitionDuration)
    {
        float brightness = backgroundColor.grayscale;
        this.transitionDuration = transitionDuration;

        // Затемнённый или осветлённый вариант
        targetUIColor = brightness < 0.5f ? backgroundColor * 1.5f : backgroundColor * 0.7f;

        // Запускаем анимацию смены цвета
        StopAllCoroutines();
        StartCoroutine(AnimateUIColorChange());
    }

    IEnumerator AnimateUIColorChange()
    {
        float elapsedTime = 0;
        Color[] startColors = new Color[uiElements.Length];

        for (int i = 0; i < uiElements.Length; i++)
            startColors[i] = uiElements[i].color;

        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / transitionDuration;

            for (int i = 0; i < uiElements.Length; i++)
                uiElements[i].color = Color.Lerp(startColors[i], targetUIColor, t);

            yield return null;
        }

        // Финальный цвет
        for (int i = 0; i < uiElements.Length; i++)
            uiElements[i].color = targetUIColor;
    }
}
