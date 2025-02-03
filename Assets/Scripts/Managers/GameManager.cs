using UnityEngine;
using YG;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Sounds")]
    [SerializeField] private AudioSource clickSound;
    [SerializeField] private AudioSource addCoinsSound;
    [SerializeField] private AudioSource addLevelSound;
    [SerializeField] private AudioSource addItemSound;
    [SerializeField] private AudioSource errorSound;
    [SerializeField] private AudioSource takeItemSound;
    [SerializeField] private AudioSource dropItemSound;

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

    public void PlayAddCoinsSound()
    {
        addCoinsSound.Play();
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
    
    public void PlayTakeItemSound()
    {
        takeItemSound.Play();
    }

    public void PlayDropItemSound()
    {
        dropItemSound.Play();
    }
}
