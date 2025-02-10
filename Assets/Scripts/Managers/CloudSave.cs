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
            new(0, -1, SlotState.Empty),
            new(1, -1, SlotState.Empty),
            new(2, -1, SlotState.Lock),
            new(3, -1, SlotState.Lock),
            new(4, -1, SlotState.Lock),
            new(5, -1, SlotState.Lock),
            new(6, -1, SlotState.Lock),
            new(7, -1, SlotState.Lock),
            new(8, -1, SlotState.Lock),
            new(9, -1, SlotState.Lock),
            new(10, -1, SlotState.Lock),
            new(11, -1, SlotState.Lock)
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

        public int GetAdvReward()
        {
            return (int)UnityEngine.Random.Range(500, YG2.saves.GetLevel() * 500);
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
    public int currentItemId;
    public SlotState state;

    public SlotData(int id, int currentItemId, SlotState state)
    {
        this.id = id;
        this.currentItemId = currentItemId;
        this.state = state;
    }
}

public enum SlotState
{
    Empty,
    Full,
    Lock
}
