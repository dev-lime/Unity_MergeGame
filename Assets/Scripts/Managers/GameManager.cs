using UnityEngine;
using YG;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void GetReward()
    {
        YG2.saves.AddCoins(YG2.saves.GetAddItemCost() * 200);
    }
}
