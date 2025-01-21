namespace YG
{
    public partial class SavesYG
    {
        private int coins = 1000;
        private Slot[] slots = null;


        public void AddCoins(int addMoney)
        {
            coins += addMoney;
        }

        public void SubCoins(int subMoney)
        {
            coins -= subMoney;
        }

        public int GetCoins()
        {
            return coins;
        }


        public void SetSlots(Slot[] slots)
        {
            this.slots = slots;
        }

        public Slot[] GetSlots()
        {
            return slots;
        }
    }
}
