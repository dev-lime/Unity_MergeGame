using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource soundSource; // Для звуковых эффектов
    [SerializeField] private AudioSource musicSource; // Для фоновой музыки

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
    [SerializeField] private Button targetButton; // Кнопка для управления музыкой
    [SerializeField] private Sprite spriteOn;  // Спрайт, когда музыка играет
    [SerializeField] private Vector2 spriteSize = new Vector2(64, 64); // Размер спрайтов

    private bool isMusicPlaying = true;
    private Image childImage; // Дочерний Image для spriteOn

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
        // Проверяем, что кнопка назначена
        if (targetButton == null)
        {
            Debug.LogError("Target Button is not assigned!");
            return;
        }

        // Назначаем обработчик нажатия кнопки
        targetButton.onClick.AddListener(ToggleMusic);

        // Создаем дочерний объект с spriteOn
        CreateChildImage(spriteOn);

        // Убедимся, что музыка играет при старте
        if (musicSource != null && isMusicPlaying)
        {
            musicSource.Play();
            musicSource.volume = 1; // Устанавливаем полную громкость
        }
    }

    void ToggleMusic()
    {
        // Переключаем состояние музыки
        if (isMusicPlaying)
        {
            StartCoroutine(FadeOutMusic()); // Плавно уменьшаем громкость и останавливаем музыку
            StartCoroutine(AnimateImageChange(0)); // Уменьшаем spriteOn до 0
        }
        else
        {
            StartCoroutine(FadeInMusic()); // Плавно увеличиваем громкость и воспроизводим музыку
            StartCoroutine(AnimateImageChange(spriteSize.y)); // Увеличиваем spriteOn до нужного размера
        }

        // Инвертируем состояние
        isMusicPlaying = !isMusicPlaying;
    }

    private System.Collections.IEnumerator FadeOutMusic()
    {
        float fadeDuration = 0.5f; // Длительность затухания
        float startVolume = musicSource.volume;

        while (musicSource.volume > 0)
        {
            musicSource.volume -= startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }

        musicSource.volume = 0;
        musicSource.Pause(); // Останавливаем музыку после затухания
    }

    private System.Collections.IEnumerator FadeInMusic()
    {
        float fadeDuration = 0.5f; // Длительность появления
        musicSource.UnPause(); // Возобновляем музыку
        musicSource.volume = 0; // Начинаем с нулевой громкости

        while (musicSource.volume < 1)
        {
            musicSource.volume += Time.deltaTime / fadeDuration;
            yield return null;
        }

        musicSource.volume = 1; // Устанавливаем полную громкость
    }

    private System.Collections.IEnumerator AnimateImageChange(float targetHeight)
    {
        // Получаем текущий дочерний объект с Image
        if (childImage == null)
        {
            Debug.LogError("Child Image is not assigned!");
            yield break;
        }

        // Анимация изменения размера
        float duration = 0.5f; // Длительность анимации
        float elapsed = 0f;
        Vector2 initialSize = childImage.rectTransform.sizeDelta;

        // Если увеличиваем спрайт, начинаем с нуля
        if (targetHeight > initialSize.y)
        {
            initialSize.y = 0;
            childImage.rectTransform.sizeDelta = initialSize;
        }

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float scaleY = Mathf.Lerp(initialSize.y, targetHeight, elapsed / duration); // Изменяем высоту
            childImage.rectTransform.sizeDelta = new Vector2(initialSize.x, scaleY);
            yield return null;
        }

        // Убедимся, что размер установлен точно
        childImage.rectTransform.sizeDelta = new Vector2(initialSize.x, targetHeight);
    }

    private void CreateChildImage(Sprite sprite)
    {
        // Удаляем старый дочерний объект (если есть)
        if (childImage != null)
        {
            Destroy(childImage.gameObject);
        }

        // Создаем новый дочерний объект
        GameObject childObject = new GameObject("ChildImage");
        childImage = childObject.AddComponent<Image>();
        childImage.sprite = sprite;
        childImage.rectTransform.sizeDelta = spriteSize; // Устанавливаем размер из инспектора

        // Центрируем объект
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
