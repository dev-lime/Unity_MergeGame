using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YG;

[System.Serializable]
public class TutorialStep
{
    public Transform startPoint; // Начальная точка перемещения
    public Transform endPoint;   // Конечная точка перемещения
}

public class TutorialManager : MonoBehaviour
{
    public List<TutorialStep> tutorialSteps; // Список этапов обучения
    public Image fingerIcon; // Иконка пальца (UI Image)
    public float animationDuration = 2f; // Длительность анимации перемещения
    public GameObject pulseWavePrefab; // Префаб для UI-волны

    private int currentStepIndex = 0; // Индекс текущего этапа
    private bool isMoving = false; // Флаг перемещения иконки
    private Vector3 startPosition; // Начальная позиция для текущего этапа
    private Vector3 endPosition; // Конечная позиция для текущего этапа
    private float animationProgress = 0f; // Прогресс анимации (от 0 до 1)
    private bool isTutorialCompleted = false; // Флаг завершения обучения

    void Start()
    {
        gameObject.SetActive(!YG2.saves.isTutorialCompleted);

        if (tutorialSteps.Count > 0)
        {
            StartStep(currentStepIndex); // Запуск первого этапа
        }
    }

    void Update()
    {
        if (isTutorialCompleted) return; // Если обучение завершено, ничего не делать

        if (isMoving)
        {
            AnimateFingerIcon(); // Анимация перемещения иконки
        }

        if (Input.GetMouseButtonDown(0)) // Обработка клика
        {
            NextStep(); // Переход к следующему этапу
        }
    }

    void StartStep(int stepIndex)
    {
        if (stepIndex >= tutorialSteps.Count)
        {
            CompleteTutorial(); // Завершение обучения, если этапы закончились
            return;
        }

        TutorialStep currentStep = tutorialSteps[stepIndex];
        startPosition = currentStep.startPoint.position; // Начальная позиция
        endPosition = currentStep.endPoint.position; // Конечная позиция
        animationProgress = 0f; // Сброс прогресса анимации
        isMoving = true; // Запуск анимации

        // Создать волну в начальной точке
        CreatePulseWave(startPosition);
    }

    void AnimateFingerIcon()
    {
        if (animationProgress < 1f)
        {
            // Увеличиваем прогресс анимации
            animationProgress += Time.deltaTime / animationDuration;

            // Применяем Easy In/Out эффект с помощью SmoothStep
            float t = Mathf.SmoothStep(0f, 1f, animationProgress);

            // Плавное перемещение иконки
            fingerIcon.rectTransform.position = Vector3.Lerp(startPosition, endPosition, t);
        }
        else
        {
            // Если анимация завершена, возвращаем иконку к начальной позиции
            animationProgress = 0f;
            fingerIcon.rectTransform.position = startPosition;

            // Создать волну в конечной точке
            CreatePulseWave(endPosition);
        }
    }

    void NextStep()
    {
        currentStepIndex++; // Переход к следующему этапу
        if (currentStepIndex < tutorialSteps.Count)
        {
            StartStep(currentStepIndex); // Запуск следующего этапа
        }
        else
        {
            CompleteTutorial(); // Завершение обучения
        }
    }

    void CompleteTutorial()
    {
        YG2.saves.isTutorialCompleted = true;
        isTutorialCompleted = true; // Обучение завершено
        isMoving = false; // Остановка анимации
        gameObject.SetActive(false); // Деактивация объекта с этим скриптом
        Debug.Log("Tutorial completed and deactivated!");
    }

    void CreatePulseWave(Vector3 position)
    {
        if (pulseWavePrefab != null)
        {
            // Создать волну на указанной позиции
            GameObject wave = Instantiate(pulseWavePrefab, position, Quaternion.identity, transform);
            wave.transform.SetAsFirstSibling(); // Поместить волну под другие UI элементы

            // Запустить анимацию волны
            StartCoroutine(AnimatePulseWave(wave));
        }
        else
        {
            Debug.LogWarning("PulseWavePrefab is not assigned!");
        }
    }

    IEnumerator AnimatePulseWave(GameObject wave)
    {
        // Получаем компоненты
        RectTransform rectTransform = wave.GetComponent<RectTransform>();
        CanvasGroup canvasGroup = wave.GetComponent<CanvasGroup>();

        if (rectTransform == null || canvasGroup == null)
        {
            Debug.LogWarning("PulseWavePrefab is missing RectTransform or CanvasGroup!");
            Destroy(wave); // Уничтожаем объект, если он не настроен правильно
            yield break;
        }

        float duration = 1f; // Длительность анимации волны
        float elapsedTime = 0f;

        Vector3 initialScale = Vector3.one * 0.5f; // Начальный масштаб
        Vector3 targetScale = Vector3.one * 2f; // Конечный масштаб

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            // Масштабирование
            rectTransform.localScale = Vector3.Lerp(initialScale, targetScale, t);

            // Уменьшение прозрачности
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, t);

            yield return null;
        }

        // Уничтожить волну после завершения анимации
        Destroy(wave);
    }
}
