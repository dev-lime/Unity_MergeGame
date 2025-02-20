using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameResources", menuName = "Game/GameResources")]
public class GameResources : ScriptableObject
{
    public List<ItemGroup> items = new ();
    public int subListSize = 4;

    private void OnEnable()
    {
        Debug.Log($"GameResources enabled. Items count: {items.Count}");
    }
}

[System.Serializable]
public class ItemGroup
{
    public List<Sprite> itemSprites;  // Вложенный список спрайтов
}
