using UnityEngine;
using YG;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Sounds")]
    [SerializeField] private AudioSource clickSound;
    [SerializeField] private AudioSource addLevelSound;
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

    private void Update()
    {
        //if (YG2.saves.GetCoins() < 0);
    }

    public void GetReward()
    {
        YG2.saves.AddCoins((int)Random.Range(100, YG2.saves.GetLevel() * 100));
    }

    public void PlayClickSound()
    {
        clickSound.Play();
    }

    public void PlayAddLevelSound()
    {
        addLevelSound.Play();
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
