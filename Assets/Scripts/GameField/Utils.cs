using UnityEngine;

public static class Utils
{
    private static GameResources gameResources;

    // ������������� ��������
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

    // ��������� ������� �� ID
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

        // ��������, ��� itemId ��������� � �������� ��������� ������
        if (itemId < 0 || itemId >= gameResources.items.Count)
        {
            Debug.LogError($"Item ID {itemId} is out of range.");
            return null;
        }

        // �������� ��������� ������ �� ItemGroup
        ItemGroup itemGroup = gameResources.items[itemId];

        // ��������, ��� ��������� ������ �� ������
        if (itemGroup == null || itemGroup.itemSprites.Count == 0)
        {
            Debug.LogError($"Item group for item ID {itemId} is empty or null.");
            return null;
        }

        // �������� ��������� ������ �� ���������� ������
        int randomIndex = Random.Range(0, itemGroup.itemSprites.Count);
        return itemGroup.itemSprites[randomIndex];
    }
}
