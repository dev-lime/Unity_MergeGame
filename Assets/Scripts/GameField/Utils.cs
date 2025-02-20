using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    private static GameResources gameResources;

    // Инициализация ресурсов
    public static void InitResources()
    {
        gameResources = Resources.Load<GameResources>("GameResources");

        if (gameResources == null)
        {
            Debug.LogError("GameResources not found in Resources folder!");
            return;
        }

        Debug.Log($"GameResources loaded. Items count: {gameResources.items.Count}");

        for (int i = 0; i < gameResources.items.Count; i++)
        {
            if (gameResources.items[i] == null)
            {
                Debug.LogError($"items[{i}] is NULL!");
            }
            else
            {
                Debug.Log($"List {i} count: {gameResources.items[i].Count}");
            }
        }
    }

    // Получение спрайта по ID
    public static Sprite GetItemVisualById(int itemId)
    {
        if (gameResources == null)
        {
            Debug.LogError("GameResources not initialized. Call InitResources() first.");
            return null;
        }

        if (gameResources.items.Count == 0)
        {
            Debug.LogError("GameResources.items empty!");
            return null;
        }

        // Проверка, что itemId находится в пределах основного списка
        if (itemId < 0 || itemId >= gameResources.items.Count)
        {
            Debug.LogError($"Item ID {itemId} is out of range.");
            return null;
        }

        // Получаем вложенный список
        List<Sprite> subList = gameResources.items[itemId];

        // Проверка, что вложенный список не пустой
        if (subList == null || subList.Count == 0)
        {
            Debug.LogError($"Sub list for item ID {itemId} is empty or null.");
            return null;
        }

        // Выбираем случайный спрайт из вложенного списка
        int randomIndex = Random.Range(0, subList.Count);
        return subList[randomIndex];
    }
}
