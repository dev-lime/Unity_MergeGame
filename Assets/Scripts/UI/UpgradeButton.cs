using UnityEngine;
using YG;

public class UpgradeButton : MonoBehaviour
{
    [SerializeField] private GameController gameController;
    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.Instance;
    }

    private void OnMouseDown()
    {
        if (YG2.saves.GetCoins() >= YG2.saves.GetAddLevelCost() && YG2.saves.GetLevel() < YG2.saves.GetMaxLevel())
        {
            gameController.Upgrade();
            gameManager.PlayClickSound();
        }
        else
        {
            gameManager.PlayErrorSound();
        }
    }
}
