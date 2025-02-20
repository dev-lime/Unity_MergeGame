using UnityEngine;
using YG;

public class UpgradeButton : MonoBehaviour
{
    private GameController gameController;
    private SoundManager soundManager;

    private void Start()
    {
        gameController = FindFirstObjectByType<GameController>();
        soundManager = SoundManager.Instance;
    }

    public void AddLevel()
    {
        YG2.InterstitialAdvShow();

        if (YG2.saves.GetCoins() >= YG2.saves.GetAddLevelCost() && YG2.saves.GetLevel() < YG2.saves.GetMaxLevel())
        {
            gameController.Upgrade();
            soundManager.PlayAddLevelSound();
        }
        else
        {
            soundManager.PlayErrorSound();
        }
    }
}
