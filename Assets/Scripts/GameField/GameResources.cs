using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameResources", menuName = "Game/GameResources")]
public class GameResources : ScriptableObject
{
    public List<List<Sprite>> items = new();
    public int subListSize = 4;

    private void OnEnable()
    {
        Debug.Log($"GameResources enabled. Items count: {items.Count}");
    }
}
