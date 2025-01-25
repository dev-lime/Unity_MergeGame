using TMPro;
using UnityEngine;
using YG;

public class WriteText : MonoBehaviour
{
    [Header("Texts")]
    public TextMeshProUGUI upgradeText;
    public TextMeshProUGUI coinsText;

    void Update()
    {
        upgradeText.text = YG2.saves.GetUpgrade().ToString();
        coinsText.text = YG2.saves.GetCoins().ToString();
    }
}
