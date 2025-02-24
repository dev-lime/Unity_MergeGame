using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIThemeManager : MonoBehaviour
{
    public Image[] uiElements; // UI �������� (������, ������ � �.�.)

    private float transitionDuration; // ������������ ��������
    private Color targetUIColor;

    public void UpdateUIColors(Color backgroundColor, float transitionDuration)
    {
        float brightness = backgroundColor.grayscale;
        this.transitionDuration = transitionDuration;

        // ���������� ��� ���������� �������
        targetUIColor = brightness < 0.5f ? backgroundColor * 1.5f : backgroundColor * 0.7f;

        // ��������� �������� ����� �����
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

        // ��������� ����
        for (int i = 0; i < uiElements.Length; i++)
            uiElements[i].color = targetUIColor;
    }
}
