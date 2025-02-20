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
        }
        else
        {
            Debug.Log($"GameResources loaded. Items count: {gameResources.items.Count}");
            for (int i = 0; i < gameResources.items.Count; i++)
            {
                Debug.Log($"List {i} count: {gameResources.items[i].itemSprites.Count}");
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

        // Получаем вложенный список из ItemGroup
        ItemGroup itemGroup = gameResources.items[itemId];

        // Проверка, что вложенный список не пустой
        if (itemGroup == null || itemGroup.itemSprites.Count == 0)
        {
            Debug.LogError($"Item group for item ID {itemId} is empty or null.");
            return null;
        }

        // Выбираем случайный спрайт из вложенного списка
        int randomIndex = Random.Range(0, itemGroup.itemSprites.Count);
        return itemGroup.itemSprites[randomIndex];
    }
}
