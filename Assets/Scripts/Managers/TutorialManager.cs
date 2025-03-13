using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YG;

[System.Serializable]
public class TutorialStep
{
    public Transform startPoint; // ��������� ����� �����������
    public Transform endPoint;   // �������� ����� �����������
}

public class TutorialManager : MonoBehaviour
{
    public List<TutorialStep> tutorialSteps; // ������ ������ ��������
    public Image fingerIcon; // ������ ������ (UI Image)
    public float animationDuration = 2f; // ������������ �������� �����������
    public GameObject pulseWavePrefab; // ������ ��� UI-�����

    private int currentStepIndex = 0; // ������ �������� �����
    private bool isMoving = false; // ���� ����������� ������
    private Vector3 startPosition; // ��������� ������� ��� �������� �����
    private Vector3 endPosition; // �������� ������� ��� �������� �����
    private float animationProgress = 0f; // �������� �������� (�� 0 �� 1)
    private bool isTutorialCompleted = false; // ���� ���������� ��������

    void Start()
    {
        gameObject.SetActive(!YG2.saves.isTutorialCompleted);

        if (tutorialSteps.Count > 0)
        {
            StartStep(currentStepIndex); // ������ ������� �����
        }
    }

    void Update()
    {
        if (isTutorialCompleted) return; // ���� �������� ���������, ������ �� ������

        if (isMoving)
        {
            AnimateFingerIcon(); // �������� ����������� ������
        }

        if (Input.GetMouseButtonDown(0)) // ��������� �����
        {
            NextStep(); // ������� � ���������� �����
        }
    }

    void StartStep(int stepIndex)
    {
        if (stepIndex >= tutorialSteps.Count)
        {
            CompleteTutorial(); // ���������� ��������, ���� ����� �����������
            return;
        }

        TutorialStep currentStep = tutorialSteps[stepIndex];
        startPosition = currentStep.startPoint.position; // ��������� �������
        endPosition = currentStep.endPoint.position; // �������� �������
        animationProgress = 0f; // ����� ��������� ��������
        isMoving = true; // ������ ��������

        // ������� ����� � ��������� �����
        CreatePulseWave(startPosition);
    }

    void AnimateFingerIcon()
    {
        if (animationProgress < 1f)
        {
            // ����������� �������� ��������
            animationProgress += Time.deltaTime / animationDuration;

            // ��������� Easy In/Out ������ � ������� SmoothStep
            float t = Mathf.SmoothStep(0f, 1f, animationProgress);

            // ������� ����������� ������
            fingerIcon.rectTransform.position = Vector3.Lerp(startPosition, endPosition, t);
        }
        else
        {
            // ���� �������� ���������, ���������� ������ � ��������� �������
            animationProgress = 0f;
            fingerIcon.rectTransform.position = startPosition;

            // ������� ����� � �������� �����
            CreatePulseWave(endPosition);
        }
    }

    void NextStep()
    {
        currentStepIndex++; // ������� � ���������� �����
        if (currentStepIndex < tutorialSteps.Count)
        {
            StartStep(currentStepIndex); // ������ ���������� �����
        }
        else
        {
            CompleteTutorial(); // ���������� ��������
        }
    }

    void CompleteTutorial()
    {
        YG2.saves.isTutorialCompleted = true;
        isTutorialCompleted = true; // �������� ���������
        isMoving = false; // ��������� ��������
        gameObject.SetActive(false); // ����������� ������� � ���� ��������
        Debug.Log("Tutorial completed and deactivated!");
    }

    void CreatePulseWave(Vector3 position)
    {
        if (pulseWavePrefab != null)
        {
            // ������� ����� �� ��������� �������
            GameObject wave = Instantiate(pulseWavePrefab, position, Quaternion.identity, transform);
            wave.transform.SetAsFirstSibling(); // ��������� ����� ��� ������ UI ��������

            // ��������� �������� �����
            StartCoroutine(AnimatePulseWave(wave));
        }
        else
        {
            Debug.LogWarning("PulseWavePrefab is not assigned!");
        }
    }

    IEnumerator AnimatePulseWave(GameObject wave)
    {
        // �������� ����������
        RectTransform rectTransform = wave.GetComponent<RectTransform>();
        CanvasGroup canvasGroup = wave.GetComponent<CanvasGroup>();

        if (rectTransform == null || canvasGroup == null)
        {
            Debug.LogWarning("PulseWavePrefab is missing RectTransform or CanvasGroup!");
            Destroy(wave); // ���������� ������, ���� �� �� �������� ���������
            yield break;
        }

        float duration = 1f; // ������������ �������� �����
        float elapsedTime = 0f;

        Vector3 initialScale = Vector3.one * 0.5f; // ��������� �������
        Vector3 targetScale = Vector3.one * 2f; // �������� �������

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            // ���������������
            rectTransform.localScale = Vector3.Lerp(initialScale, targetScale, t);

            // ���������� ������������
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, t);

            yield return null;
        }

        // ���������� ����� ����� ���������� ��������
        Destroy(wave);
    }
}
