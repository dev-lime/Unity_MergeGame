using TMPro;
using UnityEngine;
using YG;

public class UpgradeButton : MonoBehaviour
{
    public GameController gameController;

    private void OnMouseDown()
    {
        gameController.Upgrade();
    }
}
