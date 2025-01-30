using System.Collections.Generic;
using TMPro;
using UnityEngine;
using YG;

public class GameController : MonoBehaviour 
{
    public static GameController instance;

    [Header("Slots")]
    public int maxItemId = YG2.saves.GetMaxLevel();
    public int scaleModifier = 3;
    public Slot[] slots;

    [Header("Prices")]
    public TextMeshPro addItemCostText;
    public TextMeshPro upgradeCostText;
    public TextMeshPro saleItemCostText;

    private GameManager gameManager;

    private Vector3 _target;
    private ItemInfo carryingItem;

    private Dictionary<int, Slot> slotDictionary;

    private void Awake() {
        instance = this;
        Utils.InitResources();
    }

    private void Start()
    {
        gameManager = GameManager.Instance;

        addItemCostText.text = YG2.saves.GetAddItemCost().ToString();
        upgradeCostText.text = YG2.saves.GetAddLevelCost().ToString();

        slotDictionary = new Dictionary<int, Slot>();
        
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].id = i;
            slotDictionary.Add(i, slots[i]);
        }

        /*if (YG2.saves.slots != null)
        {
            slots = YG2.saves.GetSlots();
        }
        else
        {
            YG2.saves.SetSlots(slots);
        }*/
    }

    // Handle user input
    private void Update() 
    {
        if (Input.GetMouseButtonDown(0))
        {
            SendRayCast();
            //YG2.saves.SetSlots(slots);
        }

        if (Input.GetMouseButton(0) && carryingItem)
        {
            OnItemSelected();
        }

        if (Input.GetMouseButtonUp(0))
        {
            // Drop item
            SendRayCast();
        }

        if (carryingItem != null)
        {
            saleItemCostText.text = "+" + YG2.saves.GetItemSalePrice(carryingItem.itemId).ToString();
        }
        else
        {
            saleItemCostText.text = "";
        }
    }

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
                        slot.CreateItem(carryingItem.itemId);
                        Destroy(carryingItem.gameObject);
                    }
                    // We are dropping to full
                    else if (slot.state == SlotState.Full && carryingItem != null)
                    {
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
        gameManager.PlayAddCoinsSound();
        Destroy(carryingItem.gameObject);
    }

    public void Upgrade()
    {
        int addLevelCost = YG2.saves.GetAddLevelCost();

        if (YG2.saves.GetCoins() >= addLevelCost)
        {
            YG2.saves.SubCoins(addLevelCost);
            YG2.saves.AddLevel();
            addItemCostText.text = YG2.saves.GetAddItemCost().ToString();
            upgradeCostText.text = addLevelCost.ToString();
        }
    }

    public void AddRandomItem()
    {
        PlaceItemToRandomSlot();
        addItemCostText.text = YG2.saves.GetAddItemCost().ToString();
        YG2.saves.SubCoins(YG2.saves.GetAddItemCost());
    }

    public void ResetSaves()
    {
        YG2.saves.ResetSaves();
    }

    void PlaceItemToRandomSlot()
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

        slot.CreateItem(0);
    }

    public void PlaceRandomItemToRandomSlot()
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

        slot.CreateItem(Random.Range(0, YG2.saves.GetLevel()));
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

    Slot GetSlotById(int id)
    {
        return slotDictionary[id];
    }
}
