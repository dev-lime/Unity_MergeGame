using UnityEngine;
using YG;

public class AdvManager : MonoBehaviour
{
    public GameObject background;

    public void OpenPanel()
    {
        background.SetActive(true);
    }

    public void ClosePanel()
    {
        background.SetActive(false);
    }

    public void AddAdvCoins(int coins)
    {
        YG2.saves.AddCoins(coins);
        ClosePanel();
    }
}
