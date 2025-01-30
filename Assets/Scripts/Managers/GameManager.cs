using UnityEngine;
using YG;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Sounds")]
    [SerializeField] private AudioSource clickSound;
    [SerializeField] private AudioSource addCoinsSound;
    [SerializeField] private AudioSource addItemSound;
    [SerializeField] private AudioSource errorSound;

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

    public void PlayClickSound()
    {
        clickSound.Play();
    }

    public void PlayAddCoinsSound()
    {
        addCoinsSound.Play();
    }
    
    public void PlayAddItemSound()
    {
        addItemSound.Play();
    }

    public void PlayErrorSound()
    {
        errorSound.Play();
    }
}
