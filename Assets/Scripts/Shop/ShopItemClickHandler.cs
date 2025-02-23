using UnityEngine;
using UnityEngine.UI;

public class ShopItemClickHandler : MonoBehaviour
{
    public ShopManager shopManager;

    void Start()
    {
        // ��������� ���������� ������� �� ������
        Button button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(OnClick);
        }
    }

    void OnClick()
    {
        // �������� ��������� ������� � ��������
        shopManager.OnItemClick(gameObject);
    }
}
