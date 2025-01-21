using TMPro;
using UnityEngine;

public class BonusMoney : RotatingFallingObject
{
    private GameManager gameManager = GameManager.Instance;

    public Vector2 MoneyCountRange = new(50, 500);
    public TextMeshProUGUI countText;

    private int AddMoneyCount;

    private void Start()
    {
        AddMoneyCount = (int)Random.Range(MoneyCountRange.x, MoneyCountRange.y);

        if (countText != null )
        {
            countText.text = AddMoneyCount.ToString();
        }
    }

    private void OnMouseDown()
    {
        gameManager.AddPlayerMoney(AddMoneyCount);
        Destroy(gameObject);
    }
}
