using UnityEngine;
using YG;

public class AddItemButton : MonoBehaviour
{
    [SerializeField] private GameController gameController;
    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.Instance;
    }

    private void OnMouseDown()
    {
        if (YG2.saves.GetCoins() >= YG2.saves.GetAddItemCost() && !gameController.AllSlotsOccupied())
        {
            gameController.AddRandomItem();
            gameManager.PlayAddItemSound();
        }
        else
        {
            gameManager.PlayErrorSound();
        }
    }
}
