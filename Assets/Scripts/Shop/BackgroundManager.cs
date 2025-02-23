using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    public Sprite[] backgrounds;

    private SpriteRenderer backgroundRenderer;

    private void Start()
    {
        backgroundRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetBackgroundById(int id = 0)
    {
        if (backgrounds.Length == 0 || backgroundRenderer == null)
        {
            Debug.LogWarning("Array backgrounds is empty or background renderer is null");
            return;
        }

        // ���������, ����� ������ ��� � �������� �������
        if (id >= 0 && id < backgrounds.Length)
        {
            backgroundRenderer.sprite = backgrounds[id];
        }
        else
        {
            Debug.LogWarning("������ ���������� �������� ������� �� ������� ������� �����!");
        }
    }
}
