using TMPro;
using UnityEngine;
using YG;

public class BonusCoins : BonusObject
{
    public Vector2 MoneyCountRange = new(YG2.saves.GetAddItemCost() * 2, YG2.saves.GetAddItemCost() * 100);

    private int AddMoneyCount;

    private void Start()
    {
        //gameManager = GameManager.Instance;

        AddMoneyCount = (int)Random.Range(MoneyCountRange.x, MoneyCountRange.y);
    }

    private void OnMouseDown()
    {
        YG2.saves.AddCoins(AddMoneyCount);
        Destroy(gameObject);
    }
}
