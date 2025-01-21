using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    private void Update()
    {
        YG2.SaveProgress();
    }

    public void GetReward()
    {
        YG2.saves.AddCoins(1000);
    }

    public void GoToMenuScene()
    {
        SceneManager.LoadScene(0);
    }
}
