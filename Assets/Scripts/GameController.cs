using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour 
{
    public static GameController instance;
    public GameManager gameManager = GameManager.Instance;

    [Header("Buttons")]
    public Button addItemButton;

    [Header("Slots")]
    public Slot[] slots;
    public GameObject saleArea;

    [Header("Costs")]
    public int addItemCost = 20;
    public int multiplierItemCost = 100;

    private Vector3 _target;
    private ItemInfo carryingItem;

    private Dictionary<int, Slot> slotDictionary;

    private void Awake() {
        instance = this;
        Utils.InitResources();
    }

    private void Start() 
    {
        slotDictionary = new Dictionary<int, Slot>();

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].id = i;
            slotDictionary.Add(i, slots[i]);
        }
    }

    //handle user input
    private void Update() 
    {
        if (gameManager.GetPlayerMoney() < addItemCost || AllSlotsOccupied())
        {
            addItemButton.enabled = false;
        }
        else
        {
            addItemButton.enabled = true;
        }

        if (Input.GetMouseButtonDown(0))
        {
            SendRayCast();
        }

        if (Input.GetMouseButton(0) && carryingItem)
        {
            OnItemSelected();
        }

        if (Input.GetMouseButtonUp(0))
        {
            //Drop item
            SendRayCast();
        }
    }

    void SendRayCast()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        //we hit something
        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Slot"))
            {
                //we are grabbing the item in a full slot
                var slot = hit.transform.GetComponent<Slot>();
                if (slot.state == SlotState.Full && carryingItem == null)
                {
                    var itemGO = (GameObject)Instantiate(Resources.Load("Prefabs/ItemDummy"));
                    itemGO.transform.position = slot.transform.position;
                    itemGO.transform.localScale = Vector3.one * 2;

                    carryingItem = itemGO.GetComponent<ItemInfo>();
                    carryingItem.InitDummy(slot.id, slot.currentItem.id);

                    slot.ItemGrabbed();
                }
                //we are dropping an item to empty slot
                else if (slot.state == SlotState.Empty && carryingItem != null)
                {
                    slot.CreateItem(carryingItem.itemId);
                    Destroy(carryingItem.gameObject);
                }
                //we are dropping to full
                else if (slot.state == SlotState.Full && carryingItem != null)
                {
                    //check item in the slot
                    if (slot.currentItem.id == carryingItem.itemId)
                    {
                        print("merged");
                        OnItemMergedWithTarget(slot.id);
                    }
                    else
                    {
                        OnItemCarryFail();
                    }
                }
                return;
            }
            else if (hit.collider.CompareTag("SaleArea") && carryingItem != null)
            {
                SaleItem();
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

    int SaleItem()
    {
        gameManager.AddPlayerMoney((int)(math.pow(carryingItem.itemId + 1, 2)) * multiplierItemCost);
        Destroy(carryingItem.gameObject);
        return 0;
    }

    public void AddRandomItem()
    {
        PlaceRandomItem();
        gameManager.SubtractPlayerMoney(addItemCost);
    }

    void PlaceRandomItem()
    {
        if (AllSlotsOccupied())
        {
            Debug.Log("No empty slot available!");
            return;
        }

        var rand = UnityEngine.Random.Range(0, slots.Length);
        var slot = GetSlotById(rand);

        while (slot.state == SlotState.Full)
        {
            rand = UnityEngine.Random.Range(0, slots.Length);
            slot = GetSlotById(rand);
        }

        slot.CreateItem(0);
    }

    bool AllSlotsOccupied()
    {
        foreach (var slot in slots)
        {
            if (slot.state == SlotState.Empty)
            {
                //empty slot found
                return false;
            }
        }
        //no slot empty
        return true;
    }

    Slot GetSlotById(int id)
    {
        return slotDictionary[id];
    }
}
