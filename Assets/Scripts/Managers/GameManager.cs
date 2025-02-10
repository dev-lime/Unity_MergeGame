using TMPro;
using UnityEngine;
using YG;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameObject panel; // UI панель
    public TextMeshProUGUI advText;
    public float delay = 60f; // Время до появления панели
    public float pulseSpeed = 1f; // Скорость пульсации
    public float scaleMultiplier = 1.2f; // Насколько увеличивается
    
    private float timer;
    private bool isPulsing = false;
    private Vector3 originalScale;
    private int advReward = 500;

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

    void Start()
    {
        panel.SetActive(false);
        originalScale = panel.transform.localScale;
        timer = delay;
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0 && !panel.activeSelf)
        {
            ShowPanel();
        }

        if (isPulsing)
        {
            float scaleFactor = 1 + Mathf.PingPong(Time.time * pulseSpeed, scaleMultiplier - 1);
            panel.transform.localScale = originalScale * scaleFactor;
        }
    }

    void ShowPanel()
    {
        panel.SetActive(true);
        isPulsing = true;
        advReward = YG2.saves.GetAdvReward();
        advText.text = advReward.ToString();
    }

    public void HidePanel()
    {
        YG2.RewardedAdvShow("coins", GetReward);
        panel.SetActive(false);
        isPulsing = false;
        panel.transform.localScale = originalScale; // Сброс масштаба
        timer = delay; // Перезапуск таймера
    }

    public void GetReward()
    {
        YG2.saves.AddCoins(advReward);
    }
}
