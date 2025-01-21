using TMPro;
using UnityEngine;
using YG;

public class BonusMoney : RotatingFallingObject
{
    public Vector2 MoneyCountRange = new(50, 500);
    public TextMeshProUGUI countText;

    private int AddMoneyCount;

    private void Start()
    {
        gameManager = GameManager.Instance;

        AddMoneyCount = (int)Random.Range(MoneyCountRange.x, MoneyCountRange.y);

        if (countText != null )
        {
            countText.text = AddMoneyCount.ToString();
        }
    }

    private void OnMouseDown()
    {
        YG2.saves.AddCoins(AddMoneyCount);
        Destroy(gameObject);
    }
}
