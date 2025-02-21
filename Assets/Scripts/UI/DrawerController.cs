using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DrawerController : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public RectTransform drawerPanel; // Панель шторки
    public Button toggleButton; // Кнопка переключения
    public RectTransform dragHandle; // Область захвата сверху
    public float slideSpeed = 1280f; // Базовая скорость движения
    public float overshootLimit = 50f; // Максимальное перерастяжение
    public float smoothTime = 0.2f; // Время сглаживания для SmoothDamp
    public float closeThresholdSpeed = 1000f; // Порог скорости для мгновенного закрытия

    private float closedYPosition;
    private float openYPosition;
    private bool isDragging = false;
    private float startDragY;
    private float startPanelY;
    private float targetY; // Целевая позиция
    private float velocity = 0f; // Для SmoothDamp
    private float lastDragDelta = 0f; // Скорость движения

    void Start()
    {
        // Берем стартовую закрытую позицию из инспектора
        closedYPosition = drawerPanel.anchoredPosition.y;
        openYPosition = closedYPosition + drawerPanel.rect.height;
        targetY = closedYPosition;

        toggleButton.onClick.AddListener(ToggleDrawer);
    }

    void Update()
    {
        if (!isDragging)
        {
            // Плавный возврат к целевой позиции
            float newY = Mathf.SmoothDamp(drawerPanel.anchoredPosition.y, targetY, ref velocity, smoothTime);
            drawerPanel.anchoredPosition = new Vector2(drawerPanel.anchoredPosition.x, newY);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (RectTransformUtility.RectangleContainsScreenPoint(dragHandle, eventData.position))
        {
            isDragging = true;
            startDragY = eventData.position.y;
            startPanelY = drawerPanel.anchoredPosition.y;
            velocity = 0f; // Сбрасываем скорость сглаживания
            lastDragDelta = 0f; // Обнуляем скорость
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDragging)
        {
            float deltaY = eventData.position.y - startDragY;
            lastDragDelta = eventData.delta.y / Time.deltaTime; // Запоминаем скорость движения

            float newY = startPanelY + deltaY;

            // Реализация сопротивления при перерастяжении
            if (newY > openYPosition)
            {
                float overshoot = newY - openYPosition;
                newY = openYPosition + (overshoot / (1f + overshoot / overshootLimit));
            }
            else if (newY < closedYPosition)
            {
                float overshoot = closedYPosition - newY;
                newY = closedYPosition - (overshoot / (1f + overshoot / overshootLimit));
            }

            drawerPanel.anchoredPosition = new Vector2(drawerPanel.anchoredPosition.x, newY);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isDragging)
        {
            isDragging = false;

            // Если резкое движение вниз — моментально закрываем шторку
            if (lastDragDelta < -closeThresholdSpeed)
            {
                targetY = closedYPosition;
            }
            else
            {
                // Иначе фиксируем в ближайшем положении
                if (drawerPanel.anchoredPosition.y > openYPosition)
                    targetY = openYPosition;
                else if (drawerPanel.anchoredPosition.y < closedYPosition)
                    targetY = closedYPosition;
                else
                    targetY = drawerPanel.anchoredPosition.y;
            }
        }
    }

    void ToggleDrawer()
    {
        float currentY = drawerPanel.anchoredPosition.y;
        // Определяем, куда двигать шторку
        if (Mathf.Abs(currentY - openYPosition) < Mathf.Abs(currentY - closedYPosition))
        {
            targetY = closedYPosition; // Закрываем
        }
        else
        {
            targetY = openYPosition; // Открываем
        }
    }
}
