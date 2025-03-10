using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    private int currentStepIndex = 0; // ������ �������� �����
    private bool isMoving = false; // ���� ����������� ������
    private Vector3 startPosition; // ��������� ������� ��� �������� �����
    private Vector3 endPosition; // �������� ������� ��� �������� �����
    private float animationProgress = 0f; // �������� �������� (�� 0 �� 1)
    private bool isTutorialCompleted = false; // ���� ���������� ��������

    void Start()
    {
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
        isTutorialCompleted = true; // �������� ���������
        isMoving = false; // ��������� ��������
        fingerIcon.gameObject.SetActive(false); // ����������� ������ ������
        gameObject.SetActive(false); // ����������� ������� � ���� ��������
        Debug.Log("Tutorial completed and deactivated!");
    }
}
