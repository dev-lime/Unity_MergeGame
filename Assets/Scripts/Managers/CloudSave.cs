using Unity.Mathematics;

namespace YG
{
    public partial class SavesYG
    {
        public int coins = 2000;
        public int level = 1;
        public Slot[] slots = null;

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
            level++;
            YG2.SaveProgress();
        }

        public int GetLevel()
        {
            return level;
        }

        public int GetAddLevelCost()
        {
            return (int)(math.pow(GetLevel() + 1, 2) * 3000);
        }

        public int GetAddItemCost()
        {
            return 100;
        }

        public int GetItemSalePrice(int itemId)
        {
            return (int)(math.pow(itemId + 1, 2) * GetLevel() * 100);
        }

        public void SetSlots(Slot[] slots)
        {
            this.slots = slots;
            YG2.SaveProgress();
        }

        public Slot[] GetSlots()
        {
            return slots;
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
