using UnityEngine;
using TMPro;
using YG;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [Header("Item Arrays")]
    public GameObject[] shopItemsArray1; // Первый массив предметов магазина
    public GameObject[] shopItemsArray2; // Второй массив предметов магазина

    [Header("State Sprites")]
    public Sprite closedSprite; // Спрайт для состояния "закрыто"
    public Sprite availableSprite; // Спрайт для состояния "доступно"
    public Sprite selectedSprite; // Спрайт для состояния "выбрано"

    [Header("Item Prices")]
    public int[] pricesArray1; // Цены для предметов первого массива
    public int[] pricesArray2; // Цены для предметов второго массива

    private int selectedIndexArray1 = 0; // Индекс выбранного предмета в первом массиве
    private int selectedIndexArray2 = 0; // Индекс выбранного предмета во втором массиве

    void Start()
    {
        // Инициализация: обновляем состояния предметов
        UpdateItemStates(shopItemsArray1, pricesArray1, ref selectedIndexArray1);
        UpdateItemStates(shopItemsArray2, pricesArray2, ref selectedIndexArray2);
    }

    // Метод для обработки нажатия на предмет
    public void OnItemClick(GameObject clickedItem)
    {
        // Определяем, к какому массиву принадлежит предмет
        int arrayIndex = GetArrayIndex(clickedItem);

        if (arrayIndex == 0)
        {
            // Обновляем выбор в первом массиве
            int clickedIndex = GetItemIndex(shopItemsArray1, clickedItem);
            HandleItemSelection(shopItemsArray1, pricesArray1, clickedIndex, ref selectedIndexArray1);
        }
        else if (arrayIndex == 1)
        {
            // Обновляем выбор во втором массиве
            int clickedIndex = GetItemIndex(shopItemsArray2, clickedItem);
            HandleItemSelection(shopItemsArray2, pricesArray2, clickedIndex, ref selectedIndexArray2);
        }
    }

    // Метод для обработки выбора предмета
    private void HandleItemSelection(GameObject[] itemsArray, int[] prices, int clickedIndex, ref int selectedIndex)
    {
        int playerCoins = YG2.saves.GetCoins(); // Получаем текущее количество монет игрока
        int itemPrice = prices[clickedIndex];

        // Если предмет заблокирован и у игрока достаточно монет
        if (itemPrice > 0 && playerCoins >= itemPrice)
        {
            // Вычитаем монеты
            YG2.saves.SubCoins(itemPrice);
            Debug.Log($"Item purchased! Coins left: {YG2.saves.GetCoins()}");

            // Разблокируем предмет
            prices[clickedIndex] = 0; // Устанавливаем цену в 0 (предмет разблокирован)
        }

        // Обновляем выбор
        selectedIndex = clickedIndex;
        UpdateItemStates(itemsArray, prices, ref selectedIndex);
    }

    // Метод для обновления состояний предметов в массиве
    private void UpdateItemStates(GameObject[] itemsArray, int[] prices, ref int selectedIndex)
    {
        for (int i = 0; i < itemsArray.Length; i++)
        {
            // Получаем дочерний объект AvailabilityImage
            Transform availabilityImageTransform = itemsArray[i].transform.Find("AvailabilityImage");
            if (availabilityImageTransform == null)
            {
                Debug.LogWarning("AvailabilityImage not found in children of " + itemsArray[i].name);
                continue;
            }

            // Получаем компонент Image дочернего объекта
            Image availabilityImage = availabilityImageTransform.GetComponent<Image>();
            if (availabilityImage == null)
            {
                Debug.LogWarning("Image component not found on AvailabilityImage in " + itemsArray[i].name);
                continue;
            }

            // Получаем компонент TextMeshProUGUI для отображения цены
            TextMeshProUGUI costText = itemsArray[i].GetComponentInChildren<TextMeshProUGUI>();

            if (costText != null)
            {
                // Устанавливаем цену предмета
                if (prices[i] <= 0)
                {
                    costText.text = "";
                }
                else
                {
                    costText.text = prices[i].ToString();
                }

                // Проверяем, заблокирован ли предмет
                if (prices[i] > 0)
                {
                    availabilityImage.sprite = closedSprite; // Закрыто
                }
                else
                {
                    if (i == selectedIndex)
                    {
                        availabilityImage.sprite = selectedSprite; // Выбрано
                    }
                    else
                    {
                        availabilityImage.sprite = availableSprite; // Доступно
                    }
                }
            }
        }
    }

    // Метод для получения индекса предмета в массиве
    private int GetItemIndex(GameObject[] itemsArray, GameObject item)
    {
        for (int i = 0; i < itemsArray.Length; i++)
        {
            if (itemsArray[i] == item)
            {
                return i;
            }
        }
        return -1;
    }

    // Метод для определения, к какому массиву принадлежит предмет
    private int GetArrayIndex(GameObject item)
    {
        if (System.Array.IndexOf(shopItemsArray1, item) != -1)
        {
            return 0;
        }
        else if (System.Array.IndexOf(shopItemsArray2, item) != -1)
        {
            return 1;
        }
        return -1;
    }

    // Метод для получения информации о выбранном предмете
    public string GetSelectedItemInfo(int arrayIndex)
    {
        if (arrayIndex == 0)
        {
            if (selectedIndexArray1 >= 0 && selectedIndexArray1 < shopItemsArray1.Length)
            {
                return $"Selected item in array 1: {shopItemsArray1[selectedIndexArray1].name}, Price: {pricesArray1[selectedIndexArray1]}";
            }
        }
        else if (arrayIndex == 1)
        {
            if (selectedIndexArray2 >= 0 && selectedIndexArray2 < shopItemsArray2.Length)
            {
                return $"Selected item in array 2: {shopItemsArray2[selectedIndexArray2].name}, Price: {pricesArray2[selectedIndexArray2]}";
            }
        }
        return "No item selected in the specified array.";
    }
}
