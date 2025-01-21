using TMPro;
using UnityEngine;
using YG;

public class Slot : MonoBehaviour
{
    private GameManager gameManager;

    [Header("Slot Info")]
    public int id;
    public Item currentItem;
    public SlotState state = SlotState.Empty;
    [Header("Lock Slot")]
    public TextMeshPro unlockCostText;
    public int unlockSlotCost = 5000;

    private void Start()
    {
        gameManager = GameManager.Instance;

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
        UnlockSlot();
    }

    private void UnlockSlot()
    {
        if (state == SlotState.Lock && YG2.saves.GetCoins() >= unlockSlotCost)
        {
            ChangeStateTo(SlotState.Empty);
            unlockCostText.gameObject.SetActive(false);
            YG2.saves.SubCoins(unlockSlotCost);
        }
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

public enum SlotState
{
    Empty,
    Full,
    Lock
}
