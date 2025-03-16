using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using YG;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameObject panel; // UI ������
    public TextMeshProUGUI advText;
    public float delay = 60f; // ����� �� ��������� ������
    public float pulseSpeed = 1f; // �������� ���������
    public float scaleMultiplier = 1.2f; // ��������� �������������
    public Image playerPhoto;

    private float timer;
    private bool isPulsing = false;
    private Vector3 originalScale;
    private int advReward = 500;

    private string[] sequences = { "lime", "1986" };
    private string currentInput = "";

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

        if (YG2.player.photo != null)
        {
            StartCoroutine(DownloadImage(YG2.player.photo));
        }
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

        // ������������ ���� � ����������
        if (Input.anyKeyDown)
        {
            // �������� ������� �������
            string keyPressed = Input.inputString;

            // ���� ������ ���� �� ���� ������
            if (!string.IsNullOrEmpty(keyPressed))
            {
                // ��������� ������� ������� � �������� �����
                currentInput += keyPressed;

                // ���������, ��������� �� ������� ���� � ������� ����� �� �������������������
                if (!IsInputValid())
                {
                    // ���� ���� ��������, ���������� ���
                    currentInput = "";
                }

                // ���������, ��������� �� ������� ���� � ����� �� �������������������
                CheckSequences();
            }
        }
    }

    // ����� ��� ��������, ��������� �� ������� ���� � ������� ����� �� �������������������
    private bool IsInputValid()
    {
        foreach (string sequence in sequences)
        {
            // ���� ������� ���� ��������� � ������� ������������������
            if (sequence.StartsWith(currentInput))
            {
                return true;
            }
        }

        // ���� �� ���� ������������������ �� ���������� � �������� �����, ���� ��������
        return false;
    }

    // ����� ��� �������� �������� ����� �� ���������� � ��������������������
    private void CheckSequences()
    {
        foreach (string sequence in sequences)
        {
            // ���� ������� ���� ��������� � �������������������
            if (currentInput == sequence)
            {
                // ��������� �������� ��� ���� ������������������
                OnSequenceMatched(sequence);

                // ���������� ������� ����
                currentInput = "";
                return;
            }
        }
    }

    // �����, ������� ���������� ��� ���������� ����� ������������������
    private void OnSequenceMatched(string sequence)
    {
        switch (sequence)
        {
            case "lime":
                Debug.Log("lime");
                YG2.saves.AddCoins(6660666);
                break;

            case "1986":
                Debug.Log("1986");
                YG2.SetDefaultSaves();
                YG2.SaveProgress();
                break;
        }
    }

    void ShowPanel()
    {
        panel.SetActive(true);
        isPulsing = true;
        advReward = YG2.saves.GetAdvReward();
        advText.text = advReward.ToString();
    }

    public void RstSaves()
    {
        YG2.SetDefaultSaves();
        YG2.SaveProgress();
    }

    public void HidePanel()
    {
        YG2.RewardedAdvShow("coins", GetReward);
        panel.SetActive(false);
        isPulsing = false;
        panel.transform.localScale = originalScale; // ����� ��������
        timer = delay; // ���������� �������
    }

    public void GetReward()
    {
        YG2.saves.AddCoins(advReward);
    }

    IEnumerator DownloadImage(string url)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error downloading image: " + request.error);
        }
        else
        {
            // ��������� ��������
            Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;

            if (texture == null)
            {
                Debug.LogError("Failed to load texture from URL: " + url);
            }
            else
            {
                // ������ ������
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

                // ������������� ������ � Image
                if (playerPhoto != null)
                {
                    playerPhoto.sprite = sprite;

                    // �����������: ���������� Image ��������� ������� ���������
                    playerPhoto.rectTransform.sizeDelta = Vector2.zero; // �������������� ��� Mask
                }
            }
        }
    }
}
