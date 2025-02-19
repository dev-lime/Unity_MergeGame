using TMPro;
using UnityEngine;
using YG;
using System.Collections;

public class WriteText : MonoBehaviour
{
    [Header("Texts")]
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI coinsText;

    [Header("Animation")]
    public float duration = 1.0f; // Время анимации в секундах

    private long localCoins = YG2.saves.GetCoins();

    void Start()
    {
        coinsText.text = localCoins.ToString();
    }

    void Update()
    {
        levelText.text = YG2.saves.GetLevel().ToString();

        if (localCoins != YG2.saves.GetCoins())
        {
            StartCoroutine(AnimateCounter(coinsText, localCoins, YG2.saves.GetCoins(), duration));
            localCoins = YG2.saves.GetCoins();
        }
    }

    IEnumerator AnimateCounter(TextMeshProUGUI textElement, long start, long end, float time)
    {
        float elapsedTime = 0;
        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / time; // Нормализуем время (0 до 1)
            t = t * t * t * (t * (t * 6 - 15) + 10); // Улучшенный Ease-In-Out (Кривая Эрмита)

            int currentValue = Mathf.RoundToInt(Mathf.Lerp(start, end, t));
            textElement.text = currentValue.ToString();
            yield return null;
        }
        textElement.text = end.ToString(); // Финальное значение
    }
}
