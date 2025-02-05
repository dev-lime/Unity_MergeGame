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

    public void ShowAdvReward()
    {
        YG2.RewardedAdvShow("coins", GetReward);
    }

    public void GetReward()
    {
        YG2.saves.AddCoins((int)Random.Range(500, YG2.saves.GetLevel() * 500));
    }
}
