using TMPro;
using UnityEngine;
using YG;

public class WriteText : MonoBehaviour
{
    public TextMeshProUGUI coinsText;

    private GameManager gameManager;

    void Start()
    {
        gameManager = GameManager.Instance;
    }

    void Update()
    {
        if (coinsText != null)
        {
            coinsText.text = YG2.saves.GetCoins().ToString();
        }
    }
}
