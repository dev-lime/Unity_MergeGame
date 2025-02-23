using UnityEngine;
using UnityEngine.UI;

public class ShopItemClickHandler : MonoBehaviour
{
    public ShopManager shopManager;

    void Start()
    {
        // Назначаем обработчик нажатия на кнопку
        Button button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(OnClick);
        }
    }

    void OnClick()
    {
        // Передаем выбранный предмет в менеджер
        shopManager.OnItemClick(gameObject);
    }
}
