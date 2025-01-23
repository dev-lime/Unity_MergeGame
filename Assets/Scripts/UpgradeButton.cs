using TMPro;
using UnityEngine;
using YG;

public class UpgradeButton : MonoBehaviour
{
    public GameController gameController;
    public TextMeshPro upgradeCostText;

    private void OnMouseDown()
    {
        gameController.Upgrade();
        upgradeCostText.text = YG2.saves.GetUpgradeCost().ToString();
    }
}
