using NUnit.Framework.Interfaces;
using System;
using System.Linq;
using Unity.Mathematics;
using UnityEngine.SceneManagement;

namespace YG
{
    public partial class SavesYG
    {
        public int coins = 1500;
        public int level = 1;
        public SlotData[] slots = {
            new(0, null, SlotState.Empty, 0),
            new(1, null, SlotState.Empty, 0),
            new(2, null, SlotState.Lock, 1000),
            new(3, null, SlotState.Lock, 2000),
            new(4, null, SlotState.Lock, 4000),
            new(5, null, SlotState.Lock, 8000),
            new(6, null, SlotState.Lock, 16000),
            new(7, null, SlotState.Lock, 32000),
            new(8, null, SlotState.Lock, 64000),
            new(9, null, SlotState.Lock, 128000),
            new(10, null, SlotState.Lock, 256000),
            new(11, null, SlotState.Lock, 512000)
        };

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

        public SlotData GetSlotDataById(int id)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].id == id)
                    return slots[i];
            }
            return new SlotData();
        }

        public void SaveSlotData(SlotData slotData)
        {
            slots[slotData.id] = slotData;
            YG2.SaveProgress();
        }

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

[Serializable]
public struct SlotData
{
    public int id;
    public Item currentItem;
    public SlotState state;
    public int unlockSlotCost;

    public SlotData(int id, Item currentItem, SlotState state, int unlockSlotCost)
    {
        this.id = id;
        this.currentItem = currentItem;
        this.state = state;
        this.unlockSlotCost = unlockSlotCost;
    }
}

public enum SlotState
{
    Empty,
    Full,
    Lock
}
