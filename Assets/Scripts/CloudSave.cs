using Unity.Mathematics;

namespace YG
{
    public partial class SavesYG
    {
        public int coins = 2000;
        public int upgrade = 0;
        public Slot[] slots = null;

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
        }

        public int GetCoins()
        {
            return coins;
        }

        public void Upgrade()
        {
            upgrade++;
            YG2.SaveProgress();
        }

        public int GetUpgrade()
        {
            return upgrade;
        }

        public int GetUpgradeCost()
        {
            return (int)(math.pow(GetUpgrade() + 1, 2) * 1000);
        }

        public int GetAddItemCost()
        {
            return (int)(math.pow(upgrade + 1, 2)) * 100;
            //return (upgrade + 1) * 100;
        }

        public int GetItemCost(int itemId)
        {
            return (int)((int)math.pow(itemId + 1, 3) / math.sqrt(upgrade + 1) * 100);
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
