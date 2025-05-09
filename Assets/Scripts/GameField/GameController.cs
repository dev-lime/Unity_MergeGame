using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using YG;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    [Header("Slots")]
    public int scaleModifier = 9;
    public Slot[] slots;

    [Header("Prices")]
    public TextMeshProUGUI addItemCostText;
    public TextMeshProUGUI upgradeCostText;
    public TextMeshPro saleItemPriceText;

    private GameManager gameManager;
    private SoundManager soundManager;

    private readonly int maxItemId = YG2.saves.GetMaxLevel();
    private Vector3 _target;
    private ItemInfo carryingItem;

    private Dictionary<int, Slot> slotDictionary;

    [Header("Main Canvas")]
    [SerializeField] private GraphicRaycaster graphicRaycaster; // ������ �� GraphicRaycaster

    private void Awake()
    {
        instance = this;
        Utils.InitResources();
    }

    private void Start()
    {
        gameManager = GameManager.Instance;
        soundManager = SoundManager.Instance;

        addItemCostText.text = YG2.saves.GetAddItemCost().ToString();
        upgradeCostText.text = YG2.saves.GetAddLevelCost().ToString();

        slotDictionary = new Dictionary<int, Slot>();

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].id = i;
            slots[i].InitializeSlot();
            slotDictionary.Add(i, slots[i]);
        }

        // ������������� ������� GraphicRaycaster, ���� �� �� ����� � ����������
        if (graphicRaycaster == null)
        {
            graphicRaycaster = FindFirstObjectByType<GraphicRaycaster>();
        }
    }

    // Handle user input
    private void Update()
    {
        // ���������, ��������� �� ������ ���� ��� UI-���������
        bool isOverUI = IsPointerOverAnyUI();

        if (Input.GetMouseButtonDown(0)) // ����� ���� ����
        {
            if (!isOverUI) // ���� ������ �� ��� UI
            {
                SendRayCast();
            }
        }

        if (Input.GetMouseButton(0) && carryingItem) // ����������� ����� ������ ����
        {
            if (!isOverUI) // ���� ������ �� ��� UI
            {
                OnItemSelected();
            }
        }

        if (Input.GetMouseButtonUp(0)) // ���������� ����� ������ ����
        {
            if (!isOverUI) // ���� ������ �� ��� UI
            {
                SendRayCast();
            }
        }

        // ���������� ������ ���� ������� ��������
        if (carryingItem != null)
        {
            saleItemPriceText.text = "+" + YG2.saves.GetItemSalePrice(carryingItem.itemId).ToString();
        }
        else
        {
            saleItemPriceText.text = "";
        }

        // ���������� ������ ��������� ���������
        if (YG2.saves.GetLevel() < maxItemId)
        {
            upgradeCostText.text = YG2.saves.GetAddLevelCost().ToString();
        }
        else
        {
            upgradeCostText.text = "-";
        }
    }

    // ����� ��� ��������, ��������� �� ������ ��� ������� ��� ����� UI ���������
    private bool IsPointerOverAnyUI()
    {
        // ������� ������ ��� Raycast
        PointerEventData pointerData = new PointerEventData(EventSystem.current);
        pointerData.position = Input.mousePosition; // ������� ������� ��� �������

        // ������ ����������� Raycast
        System.Collections.Generic.List<RaycastResult> results = new System.Collections.Generic.List<RaycastResult>();

        // ��������� Raycast
        graphicRaycaster.Raycast(pointerData, results);

        // ���� ���� ���� �� ���� ���������, ������ ������ ��� UI ���������
        return results.Count > 0;
    }

    // ��������� ��� �������� ��� ���������
    void SendRayCast()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        // We hit something
        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Slot"))
            {
                var slot = hit.transform.GetComponent<Slot>();
                if (slot.state != SlotState.Lock)
                {
                    // We are grabbing the item in a full slot
                    if (slot.state == SlotState.Full && carryingItem == null)
                    {
                        soundManager.PlayTakeItemSound();

                        var itemGO = (GameObject)Instantiate(Resources.Load("Prefabs/ItemDummy"));
                        itemGO.transform.position = slot.transform.position;
                        itemGO.transform.localScale = Vector3.one * scaleModifier;

                        carryingItem = itemGO.GetComponent<ItemInfo>();
                        carryingItem.InitDummy(slot.id, slot.currentItem.id);

                        slot.ItemGrabbed();
                    }
                    // We are dropping an item to empty slot
                    else if (slot.state == SlotState.Empty && carryingItem != null)
                    {
                        soundManager.PlayDropItemSound();

                        slot.CreateItem(carryingItem.itemId);
                        Destroy(carryingItem.gameObject);
                    }
                    // We are dropping to full
                    else if (slot.state == SlotState.Full && carryingItem != null)
                    {
                        soundManager.PlayDropItemSound();

                        // Check item in the slot
                        if (slot.currentItem.id == carryingItem.itemId && carryingItem.itemId < maxItemId)
                        {
                            OnItemMergedWithTarget(slot.id);
                        }
                        else
                        {
                            OnItemCarryFail();
                        }
                    }
                }
                else if (carryingItem != null)
                {
                    OnItemCarryFail();
                }
                return;
            }
            else if (hit.collider.CompareTag("SaleArea") && carryingItem != null)
            {
                SellItem();
                return;
            }
        }

        if (!carryingItem)
        {
            return;
        }
        OnItemCarryFail();
    }

    // ��������� ������ �������� ��� ���������
    void OnItemSelected()
    {
        _target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _target.z = 0;
        var delta = 10 * Time.deltaTime;

        delta *= Vector3.Distance(transform.position, _target);
        carryingItem.transform.position = Vector3.MoveTowards(carryingItem.transform.position, _target, delta);
    }

    void OnItemMergedWithTarget(int targetSlotId)
    {
        var slot = GetSlotById(targetSlotId);
        Destroy(slot.currentItem.gameObject);

        slot.CreateItem(carryingItem.itemId + 1);

        Destroy(carryingItem.gameObject);
    }

    void OnItemCarryFail()
    {
        var slot = GetSlotById(carryingItem.slotId);
        slot.CreateItem(carryingItem.itemId);
        Destroy(carryingItem.gameObject);
    }

    void SellItem()
    {
        YG2.saves.AddCoins(YG2.saves.GetItemSalePrice(carryingItem.itemId));
        soundManager.PlayAddCoinsSound();
        Destroy(carryingItem.gameObject);
    }

    public void Upgrade()
    {
        int addLevelCost = YG2.saves.GetAddLevelCost();

        if (YG2.saves.GetCoins() >= addLevelCost)
        {
            YG2.saves.SubCoins(addLevelCost);
            YG2.saves.AddLevel();
        }
    }

    public void ShowInterstitialAdv()
    {
        YG2.InterstitialAdvShow();
    }

    public void AddRandomItem()
    {
        PlaceItemToRandomSlot(0);
        addItemCostText.text = YG2.saves.GetAddItemCost().ToString();
        YG2.saves.SubCoins(YG2.saves.GetAddItemCost());
    }

    public void ResetSaves()
    {
        YG2.saves.ResetSaves();
    }

    public void BuyCoins(int coins)
    {
        YG2.saves.AddCoins(coins);
    }

    void PlaceItemToRandomSlot(int id)
    {
        if (AllSlotsOccupied())
        {
            return;
        }

        var rand = Random.Range(0, slots.Length);
        var slot = GetSlotById(rand);

        while (slot.state != SlotState.Empty)
        {
            rand = Random.Range(0, slots.Length);
            slot = GetSlotById(rand);
        }

        slot.CreateItem(id);
    }

    public bool PlaceRandomItemToRandomSlot()
    {
        if (AllSlotsOccupied())
        {
            return false;
        }

        var rand = Random.Range(0, slots.Length);
        var slot = GetSlotById(rand);

        while (slot.state != SlotState.Empty)
        {
            rand = Random.Range(0, slots.Length);
            slot = GetSlotById(rand);
        }

        slot.CreateItem(Random.Range(0, YG2.saves.GetLevel()));

        return true;
    }

    public bool AllSlotsOccupied()
    {
        foreach (var slot in slots)
        {
            if (slot.state == SlotState.Empty)
            {
                // Empty slot found
                return false;
            }
        }
        // No slot empty
        return true;
    }

    public bool GetCarringItem()
    {
        if (carryingItem == null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    Slot GetSlotById(int id)
    {
        return slotDictionary[id];
    }
}
