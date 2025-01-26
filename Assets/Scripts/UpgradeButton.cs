using TMPro;
using UnityEngine;
using YG;

public class UpgradeButton : MonoBehaviour
{
    public GameController gameController;
    public AudioSource clickSound;

    private void OnMouseDown()
    {
        if (YG2.saves.GetCoins() >= YG2.saves.GetUpgradeCost())
        {
            gameController.Upgrade();
            clickSound.Play();
        }
    }
}
