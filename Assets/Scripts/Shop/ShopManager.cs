using UnityEngine;
using TMPro;
using YG;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [Header("Item Arrays")]
    public GameObject[] shopItemsArray1; // ������ ������ ��������� ��������
    public GameObject[] shopItemsArray2; // ������ ������ ��������� ��������

    [Header("State Sprites")]
    public Sprite closedSprite; // ������ ��� ��������� "�������"
    public Sprite availableSprite; // ������ ��� ��������� "��������"
    public Sprite selectedSprite; // ������ ��� ��������� "�������"

    [Header("Item Prices")]
    public int[] pricesArray1; // ���� ��� ��������� ������� �������
    public int[] pricesArray2; // ���� ��� ��������� ������� �������

    private int selectedIndexArray1 = 0; // ������ ���������� �������� � ������ �������
    private int selectedIndexArray2 = 0; // ������ ���������� �������� �� ������ �������

    void Start()
    {
        // �������������: ��������� ��������� ���������
        UpdateItemStates(shopItemsArray1, pricesArray1, ref selectedIndexArray1);
        UpdateItemStates(shopItemsArray2, pricesArray2, ref selectedIndexArray2);
    }

    // ����� ��� ��������� ������� �� �������
    public void OnItemClick(GameObject clickedItem)
    {
        // ����������, � ������ ������� ����������� �������
        int arrayIndex = GetArrayIndex(clickedItem);

        if (arrayIndex == 0)
        {
            // ��������� ����� � ������ �������
            int clickedIndex = GetItemIndex(shopItemsArray1, clickedItem);
            HandleItemSelection(shopItemsArray1, pricesArray1, clickedIndex, ref selectedIndexArray1);
        }
        else if (arrayIndex == 1)
        {
            // ��������� ����� �� ������ �������
            int clickedIndex = GetItemIndex(shopItemsArray2, clickedItem);
            HandleItemSelection(shopItemsArray2, pricesArray2, clickedIndex, ref selectedIndexArray2);
        }
    }

    // ����� ��� ��������� ������ ��������
    private void HandleItemSelection(GameObject[] itemsArray, int[] prices, int clickedIndex, ref int selectedIndex)
    {
        int playerCoins = YG2.saves.GetCoins(); // �������� ������� ���������� ����� ������
        int itemPrice = prices[clickedIndex];

        // ���� ������� ������������ � � ������ ���������� �����
        if (itemPrice > 0 && playerCoins >= itemPrice)
        {
            // �������� ������
            YG2.saves.SubCoins(itemPrice);
            Debug.Log($"Item purchased! Coins left: {YG2.saves.GetCoins()}");

            // ������������ �������
            prices[clickedIndex] = 0; // ������������� ���� � 0 (������� �������������)
        }

        // ��������� �����
        selectedIndex = clickedIndex;
        UpdateItemStates(itemsArray, prices, ref selectedIndex);
    }

    // ����� ��� ���������� ��������� ��������� � �������
    private void UpdateItemStates(GameObject[] itemsArray, int[] prices, ref int selectedIndex)
    {
        for (int i = 0; i < itemsArray.Length; i++)
        {
            // �������� �������� ������ AvailabilityImage
            Transform availabilityImageTransform = itemsArray[i].transform.Find("AvailabilityImage");
            if (availabilityImageTransform == null)
            {
                Debug.LogWarning("AvailabilityImage not found in children of " + itemsArray[i].name);
                continue;
            }

            // �������� ��������� Image ��������� �������
            Image availabilityImage = availabilityImageTransform.GetComponent<Image>();
            if (availabilityImage == null)
            {
                Debug.LogWarning("Image component not found on AvailabilityImage in " + itemsArray[i].name);
                continue;
            }

            // �������� ��������� TextMeshProUGUI ��� ����������� ����
            TextMeshProUGUI costText = itemsArray[i].GetComponentInChildren<TextMeshProUGUI>();

            if (costText != null)
            {
                // ������������� ���� ��������
                if (prices[i] <= 0)
                {
                    costText.text = "";
                }
                else
                {
                    costText.text = prices[i].ToString();
                }

                // ���������, ������������ �� �������
                if (prices[i] > 0)
                {
                    availabilityImage.sprite = closedSprite; // �������
                }
                else
                {
                    if (i == selectedIndex)
                    {
                        availabilityImage.sprite = selectedSprite; // �������
                    }
                    else
                    {
                        availabilityImage.sprite = availableSprite; // ��������
                    }
                }
            }
        }
    }

    // ����� ��� ��������� ������� �������� � �������
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

    // ����� ��� �����������, � ������ ������� ����������� �������
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

    // ����� ��� ��������� ���������� � ��������� ��������
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
