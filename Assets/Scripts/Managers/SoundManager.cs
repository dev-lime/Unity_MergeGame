using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource soundSource; // ��� �������� ��������
    [SerializeField] private AudioSource musicSource; // ��� ������� ������

    [Header("Sound Clips")]
    [SerializeField] private AudioClip[] errorSounds;
    [SerializeField] private AudioClip[] addCoinsSounds;
    [SerializeField] private AudioClip[] clickSounds;
    [SerializeField] private AudioClip[] addLevelSounds;
    [SerializeField] private AudioClip[] addItemSounds;
    [SerializeField] private AudioClip[] takeItemSounds;
    [SerializeField] private AudioClip[] dropItemSounds;
    [SerializeField] private AudioClip[] restartSounds;

    [Header("Background Music")]
    [SerializeField] private Button targetButton; // ������ ��� ���������� �������
    [SerializeField] private Sprite spriteOn;  // ������, ����� ������ ������
    [SerializeField] private Vector2 spriteSize = new Vector2(64, 64); // ������ ��������

    private bool isMusicPlaying = true;
    private Image childImage; // �������� Image ��� spriteOn

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

        if (musicSource == null)
            musicSource = gameObject.AddComponent<AudioSource>();
    }

    void Start()
    {
        // ���������, ��� ������ ���������
        if (targetButton == null)
        {
            Debug.LogError("Target Button is not assigned!");
            return;
        }

        // ��������� ���������� ������� ������
        targetButton.onClick.AddListener(ToggleMusic);

        // ������� �������� ������ � spriteOn
        CreateChildImage(spriteOn);

        // ��������, ��� ������ ������ ��� ������
        if (musicSource != null && isMusicPlaying)
        {
            musicSource.Play();
            musicSource.volume = 1; // ������������� ������ ���������
        }
    }

    void ToggleMusic()
    {
        // ����������� ��������� ������
        if (isMusicPlaying)
        {
            StartCoroutine(FadeOutMusic()); // ������ ��������� ��������� � ������������� ������
            StartCoroutine(AnimateImageChange(0)); // ��������� spriteOn �� 0
        }
        else
        {
            StartCoroutine(FadeInMusic()); // ������ ����������� ��������� � ������������� ������
            StartCoroutine(AnimateImageChange(spriteSize.y)); // ����������� spriteOn �� ������� �������
        }

        // ����������� ���������
        isMusicPlaying = !isMusicPlaying;
    }

    private System.Collections.IEnumerator FadeOutMusic()
    {
        float fadeDuration = 0.5f; // ������������ ���������
        float startVolume = musicSource.volume;

        while (musicSource.volume > 0)
        {
            musicSource.volume -= startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }

        musicSource.volume = 0;
        musicSource.Pause(); // ������������� ������ ����� ���������
    }

    private System.Collections.IEnumerator FadeInMusic()
    {
        float fadeDuration = 0.5f; // ������������ ���������
        musicSource.UnPause(); // ������������ ������
        musicSource.volume = 0; // �������� � ������� ���������

        while (musicSource.volume < 1)
        {
            musicSource.volume += Time.deltaTime / fadeDuration;
            yield return null;
        }

        musicSource.volume = 1; // ������������� ������ ���������
    }

    private System.Collections.IEnumerator AnimateImageChange(float targetHeight)
    {
        // �������� ������� �������� ������ � Image
        if (childImage == null)
        {
            Debug.LogError("Child Image is not assigned!");
            yield break;
        }

        // �������� ��������� �������
        float duration = 0.5f; // ������������ ��������
        float elapsed = 0f;
        Vector2 initialSize = childImage.rectTransform.sizeDelta;

        // ���� ����������� ������, �������� � ����
        if (targetHeight > initialSize.y)
        {
            initialSize.y = 0;
            childImage.rectTransform.sizeDelta = initialSize;
        }

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float scaleY = Mathf.Lerp(initialSize.y, targetHeight, elapsed / duration); // �������� ������
            childImage.rectTransform.sizeDelta = new Vector2(initialSize.x, scaleY);
            yield return null;
        }

        // ��������, ��� ������ ���������� �����
        childImage.rectTransform.sizeDelta = new Vector2(initialSize.x, targetHeight);
    }

    private void CreateChildImage(Sprite sprite)
    {
        // ������� ������ �������� ������ (���� ����)
        if (childImage != null)
        {
            Destroy(childImage.gameObject);
        }

        // ������� ����� �������� ������
        GameObject childObject = new GameObject("ChildImage");
        childImage = childObject.AddComponent<Image>();
        childImage.sprite = sprite;
        childImage.rectTransform.sizeDelta = spriteSize; // ������������� ������ �� ����������

        // ���������� ������
        childObject.transform.SetParent(targetButton.transform, false);
        childImage.rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        childImage.rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        childImage.rectTransform.pivot = new Vector2(0.5f, 0.5f);
        childImage.rectTransform.anchoredPosition = Vector2.zero;
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
