using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("HUD")]
    public TextMeshProUGUI PlayerMoneyText;

    private int PlayerMoney = 1000;

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

    private void Start()
    {
        //Screen.SetResolution(720, 1280, true);
    }

    private void Update()
    {
        PlayerMoneyText.text = PlayerMoney.ToString();
    }

    public void AddPlayerMoney(int addMoney)
    {
        PlayerMoney += addMoney;
    }

    public void SubtractPlayerMoney(int subMoney)
    {
        PlayerMoney -= subMoney;
    }

    public int GetPlayerMoney()
    {
        return PlayerMoney;
    }
}
