using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PriceTable", menuName = "Game Data/Price Table")]
public class PriceTable : ScriptableObject
{
    public List<int> prices = new List<int>();

    public int GetPrice(int id)
    {
        return (id >= 0 && id < prices.Count) ? prices[id] : -1; // Проверка границ
    }
}
