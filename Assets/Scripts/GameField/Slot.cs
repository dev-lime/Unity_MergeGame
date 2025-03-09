using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using YG;

public class Slot : MonoBehaviour
{
    public GameObject shadow;

    [Header("Slot Info")]
    public int id;
    public Item currentItem;
    public SlotState state = SlotState.Empty;
    public float scalemodifier = 3f;

    [Header("Lock Slot")]
    [SerializeField] private TextMeshPro unlockCostText;
    [SerializeField] private int unlockSlotCost = 5000;

    private GameManager gameManager;
    private GameController gameController;
    private SoundManager soundManager;

    private bool isFading = false;
    private readonly float fadeSpeed = 3f; // Скорость исчезновения

    [Header("Main Canvas")]
    [SerializeField] private GraphicRaycaster graphicRaycaster; // Ссылка на GraphicRaycaster

    private void Start()
    {
        gameManager = GameManager.Instance;
        gameController = FindFirstObjectByType<GameController>();
        soundManager = SoundManager.Instance;

        SetAlpha(unlockCostText, 1f);

        // Если graphicRaycaster не задан в инспекторе, находим его автоматически
        if (graphicRaycaster == null)
        {
            graphicRaycaster = FindFirstObjectByType<GraphicRaycaster>();
        }
    }

    private void OnEnable()
    {
        YG2.saves.OnResetSaves += InitializeSlot;
    }

    private void OnDisable()
    {
        YG2.saves.OnResetSaves -= InitializeSlot;
    }

    void Update()
    {
        if (isFading)
        {
            float alpha = Mathf.Clamp01(unlockCostText.color.a - fadeSpeed * Time.deltaTime);
            SetAlpha(unlockCostText, alpha);

            if (alpha <= 0)
            {
                unlockCostText.gameObject.SetActive(false);
                isFading = false;
                SetAlpha(unlockCostText, 255);
            }
        }
    }

    public void InitializeSlot()
    {
        SlotData YGSlots = YG2.saves.GetSlotDataById(id);

        state = YGSlots.state;

        if (YGSlots.currentItemId == -1)
        {
            if (currentItem != null)
                Destroy(currentItem.gameObject);
        }
        else
        {
            CreateItem(YGSlots.currentItemId);
        }

        unlockCostText.text = unlockSlotCost.ToString();
        unlockCostText.gameObject.SetActive(state == SlotState.Lock);
    }

    void SaveSlotData()
    {
        int currentItemId = -1;
        if (state == SlotState.Full) currentItemId = currentItem.id;
        YG2.saves.SaveSlotData(new SlotData(id, currentItemId, state));
    }

    public void CreateItem(int id)
    {
        var itemGO = (GameObject)Instantiate(Resources.Load("Prefabs/Item"));
        itemGO.transform.SetParent(this.transform);
        itemGO.transform.localPosition = Vector3.zero;
        itemGO.transform.localScale = Vector3.one * scalemodifier;

        currentItem = itemGO.GetComponent<Item>();
        currentItem.Init(id, this);

        ChangeStateTo(SlotState.Full);
    }

    private void ChangeStateTo(SlotState targetState)
    {
        state = targetState;
        SaveSlotData();
    }

    public void ItemGrabbed()
    {
        Destroy(currentItem.gameObject);
        ChangeStateTo(SlotState.Empty);
    }

    private void OnMouseDown()
    {
        // Проверяем, находится ли курсор или касание над любым UI элементом
        if (IsPointerOverAnyUI())
        {
            return; // Если да, не обрабатываем нажатие
        }

        if (state == SlotState.Lock)
        {
            if (YG2.saves.GetCoins() >= unlockSlotCost && !isFading)
            {
                soundManager.PlayClickSound();
                isFading = true;
                UnlockSlot();
            }
            else
            {
                soundManager.PlayErrorSound();
            }
        }
    }

    private void UnlockSlot()
    {
        ChangeStateTo(SlotState.Empty);
        YG2.saves.SubCoins(unlockSlotCost);
    }

    public void OnMouseEnter()
    {
        /*if (IsPointerOverAnyUI())
        {
            return;
        }*/

        if (!gameController.GetCarringItem() || state == SlotState.Empty)
            shadow.SetActive(true);
    }

    public void OnMouseExit()
    {
        if (shadow.activeSelf)
            shadow.SetActive(false);
    }

    // Метод для проверки, находится ли курсор или касание над любым UI элементом
    private bool IsPointerOverAnyUI()
    {
        // Создаем данные для Raycast
        PointerEventData pointerData = new PointerEventData(EventSystem.current);
        pointerData.position = Input.mousePosition; // Позиция курсора или касания

        // Список результатов Raycast
        System.Collections.Generic.List<RaycastResult> results = new System.Collections.Generic.List<RaycastResult>();

        // Выполняем Raycast
        graphicRaycaster.Raycast(pointerData, results);

        // Если есть хотя бы один результат, значит курсор над UI элементом
        return results.Count > 0;
    }

    // Метод для установки прозрачности (альфа-канала) на объект и все его дочерние элементы
    void SetAlpha(TextMeshPro text, float alpha)
    {
        Color color = text.color;
        text.color = new Color(color.r, color.g, color.b, alpha);

        foreach (Transform child in text.transform)
        {
            TextMeshPro childText = child.GetComponent<TextMeshPro>();
            if (childText != null)
            {
                SetAlpha(childText, alpha);
            }

            SpriteRenderer childSprite = child.GetComponent<SpriteRenderer>();
            if (childSprite != null)
            {
                SetAlpha(childSprite, alpha);
            }
        }
    }

    // Метод для установки прозрачности (альфа-канала) для объекта с SpriteRenderer
    void SetAlpha(SpriteRenderer sprite, float alpha)
    {
        Color color = sprite.color;
        sprite.color = new Color(color.r, color.g, color.b, alpha);
    }
}
