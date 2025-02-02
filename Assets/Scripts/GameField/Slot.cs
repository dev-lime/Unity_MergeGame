using TMPro;
using UnityEngine;
using YG;

public class Slot : MonoBehaviour
{
    [Header("Slot Info")]
    public int id;
    public Item currentItem;
    public SlotState state = SlotState.Empty;

    [Header("Lock Slot")]
    [SerializeField] private TextMeshPro unlockCostText;
    [SerializeField] private int unlockSlotCost = 5000;

    private GameManager gameManager;

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
            if (UnlockSlot())
            {
                gameManager.PlayClickSound();
            }
            else
            {
                gameManager.PlayErrorSound();
            }
        }
    }

    private bool UnlockSlot()
    {
        if (state == SlotState.Lock && YG2.saves.GetCoins() >= unlockSlotCost)
        {
            ChangeStateTo(SlotState.Empty);
            unlockCostText.gameObject.SetActive(false);
            YG2.saves.SubCoins(unlockSlotCost);
            return true;
        }
        else
        {
            return false;
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
