using Unity.Mathematics;

namespace YG
{
    public partial class SavesYG
    {
        private int coins = 2000;
        private int upgrade = 0;
        private Slot[] slots = null;

        public void AddCoins(int addMoney)
        {
            coins += addMoney;
            YG2.SaveProgress();
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
            return (int)(math.pow((GetUpgrade() + 1), 2) * 100);
        }

        public int GetAddItemCost()
        {
            return (int)((upgrade + 1) * 100);
        }

        public int GetItemCost(int itemId)
        {
            return (int)(math.pow(itemId + 1, 2)) * 100;
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
    }
}
