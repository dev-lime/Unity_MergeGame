using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource soundSource;

    [Header("Sound Clips")]
    [SerializeField] private AudioClip[] errorSounds;
    [SerializeField] private AudioClip[] addCoinsSounds;
    [SerializeField] private AudioClip[] clickSounds;
    [SerializeField] private AudioClip[] addLevelSounds;
    [SerializeField] private AudioClip[] addItemSounds;
    [SerializeField] private AudioClip[] takeItemSounds;
    [SerializeField] private AudioClip[] dropItemSounds;
    [SerializeField] private AudioClip[] restartSounds;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        Debug.Log("SoundManager initialized.");

        if (soundSource == null)
            soundSource = gameObject.AddComponent<AudioSource>();
    }

    private void PlayRandomSound(AudioClip[] clips)
    {
        if (clips == null || clips.Length == 0) return;

        AudioClip clip = clips.Length == 1 ? clips[0] : clips[Random.Range(0, clips.Length)];
        soundSource.PlayOneShot(clip);
    }

    public void PlayErrorSound()
    {
        PlayRandomSound(errorSounds);
    }

    public void PlayClickSound()
    {
        PlayRandomSound(clickSounds);
    }

    public void PlayAddCoinsSound()
    {
        PlayRandomSound(addCoinsSounds);
    }

    public void PlayAddLevelSound()
    {
        PlayRandomSound(addLevelSounds);
    }

    public void PlayAddItemSound()
    {
        PlayRandomSound(addItemSounds);
    }

    public void PlayTakeItemSound()
    {
        PlayRandomSound(takeItemSounds);
    }

    public void PlayDropItemSound()
    {
        PlayRandomSound(dropItemSounds);
    }

    public void PlayRestartSound()
    {
        PlayRandomSound(restartSounds);
    }
}
