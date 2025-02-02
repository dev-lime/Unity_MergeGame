using System;
using System.Linq;
using Unity.Mathematics;

namespace YG
{
    public partial class SavesYG
    {
        public int coins = 1500;
        public int level = 1;
        //public SlotData[] slots = null;

        private readonly int maxLevel = 51;

        public void AddCoins(int addMoney)
        {
            coins += addMoney;
            YG2.SaveProgress();
            UpdateRecord(YG2.saves.GetCoins());
        }

        public void SubCoins(int subMoney)
        {
            coins -= subMoney;
            YG2.SaveProgress();
            UpdateRecord(YG2.saves.GetCoins());
        }

        public int GetCoins()
        {
            return coins;
        }

        public void AddLevel()
        {
            if (GetLevel() < GetMaxLevel())
            {
                level++;
                YG2.SaveProgress();
            }
        }

        public int GetLevel()
        {
            return level;
        }

        public int GetAddLevelCost()
        {
            return (int)(math.pow(GetLevel(), 1.5f) * 1000);
        }

        public int GetAddItemCost()
        {
            return 100;
        }

        public int GetItemSalePrice(int itemId)
        {
            return (int)(math.pow(itemId + 1, 2) * math.sqrt(GetLevel()) * 50);
        }

        /*public void SetSlots(int id, Item currentItem, SlotState state)
        {
            SlotData slotData = new SlotData(id, currentItem, state);
            slots.Append(slotData);
            YG2.SaveProgress();
        }

        public SlotData[] GetSlots()
        {
            return slots;
        }*/

        public int GetMaxLevel()
        {
            return maxLevel;
        }

        public void ResetSaves()
        {
            YG2.SetDefaultSaves();
            YG2.SaveProgress();
        }

        public void UpdateRecord(int coins)
        {
            YG2.SetLeaderboard("score", coins);
        }
    }
}

/*[Serializable]
public struct SlotData
{
    public int id;
    public Item currentItem;
    public SlotState state;

    public SlotData(int id, Item currentItem, SlotState state)
    {
        this.id = id;
        this.currentItem = currentItem;
        this.state = state;
    }
}*/

public enum SlotState
{
    Empty,
    Full,
    Lock
}
