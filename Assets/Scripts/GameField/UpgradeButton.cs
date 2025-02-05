using UnityEngine;
using YG;

public class UpgradeButton : MonoBehaviour
{
    public GameObject shadow;

    private GameController gameController;
    private SoundManager soundManager;

    private void Start()
    {
        gameController = FindFirstObjectByType<GameController>();
        soundManager = SoundManager.Instance;
    }

    private void OnMouseDown()
    {
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

    public void OnMouseEnter()
    {
        shadow.SetActive(true);
    }

    public void OnMouseExit()
    {
        shadow.SetActive(false);
    }
}
