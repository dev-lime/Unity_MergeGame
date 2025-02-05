using TMPro;
using UnityEngine;
using YG;

public class Slot : MonoBehaviour
{
    public GameObject shadow;

    [Header("Slot Info")]
    public int id;
    public Item currentItem;
    public SlotState state = SlotState.Empty;

    [Header("Lock Slot")]
    [SerializeField] private TextMeshPro unlockCostText;
    [SerializeField] private int unlockSlotCost = 5000;

    private GameManager gameManager;
    private SoundManager soundManager;

    private Renderer objectRenderer;
    private bool isFading = false;
    private float fadeSpeed = 3f; // Скорость исчезновения

    private void Start()
    {
        gameManager = GameManager.Instance;
        soundManager = SoundManager.Instance;

        SetAlpha(unlockCostText, 1f);

        unlockCostText.text = unlockSlotCost.ToString();
        if (state == SlotState.Lock)
        {
            unlockCostText.gameObject.SetActive(true);
        }
        else
        {
            unlockCostText.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (isFading)
        {
            // Постепенное уменьшение прозрачности для объекта и всех его дочерних объектов
            float alpha = Mathf.Clamp01(unlockCostText.color.a - fadeSpeed * Time.deltaTime);
            SetAlpha(unlockCostText, alpha);

            // Если прозрачность стала 0, отключаем объект и все его дочерние элементы
            if (alpha <= 0)
            {
                unlockCostText.gameObject.SetActive(false);
                isFading = false;
                UnlockSlot();
            }
        }
    }

    public void LoadSlot(int id, Item currentItem, SlotState state)
    {
        if (this.id == id)
        {
            this.currentItem = currentItem;
            this.state = state;
        }
    }

    public void CreateItem(int id) 
    {
        var itemGO = (GameObject)Instantiate(Resources.Load("Prefabs/Item"));
        
        itemGO.transform.SetParent(this.transform);
        itemGO.transform.localPosition = Vector3.zero;
        itemGO.transform.localScale = Vector3.one;

        currentItem = itemGO.GetComponent<Item>();
        currentItem.Init(id, this);

        ChangeStateTo(SlotState.Full);
    }

    private void ChangeStateTo(SlotState targetState)
    {
        state = targetState;
    }

    public void ItemGrabbed()
    {
        Destroy(currentItem.gameObject);
        ChangeStateTo(SlotState.Empty);
    }

    private void OnMouseDown()
    {
        if (state == SlotState.Lock)
        {
            if (YG2.saves.GetCoins() >= unlockSlotCost && !isFading)
            {
                soundManager.PlayClickSound();
                isFading = true;
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
        shadow.SetActive(true);
    }

    public void OnMouseExit()
    {
        shadow.SetActive(false);
    }

    // Метод для установки прозрачности (альфа-канала) на объект и все его дочерние элементы
    void SetAlpha(TextMeshPro text, float alpha)
    {
        // Устанавливаем прозрачность для текущего объекта
        Color color = text.color;
        text.color = new Color(color.r, color.g, color.b, alpha);

        // Рекурсивно обрабатываем все дочерние объекты
        foreach (Transform child in text.transform)
        {
            TextMeshPro childText = child.GetComponent<TextMeshPro>();
            if (childText != null)
            {
                SetAlpha(childText, alpha); // Рекурсивно меняем прозрачность дочернего объекта
            }

            // Для дочерних объектов с компонентом SpriteRenderer
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
        // Устанавливаем прозрачность для спрайта
        Color color = sprite.color;
        sprite.color = new Color(color.r, color.g, color.b, alpha);
    }

    private void ReceiveItem(int id)
    {
        switch (state)
        {
            case SlotState.Empty: 

                CreateItem(id);
                ChangeStateTo(SlotState.Full);
                break;

            case SlotState.Full: 
                if (currentItem.id == id)
                {
                    //Merged
                    Debug.Log("Merged");
                }
                else
                {
                    //Push item back
                    Debug.Log("Push back");
                }
            break;
        }
    }
}
