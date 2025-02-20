using UnityEngine;
using YG;

public class AddItemButton : MonoBehaviour
{
    private GameController gameController;
    private SoundManager soundManager;

    private void Start()
    {
        gameController = FindFirstObjectByType<GameController>();
        soundManager = SoundManager.Instance;
    }

    public void AddItem()
    {
        if (YG2.saves.GetCoins() >= YG2.saves.GetAddItemCost() && !gameController.AllSlotsOccupied())
        {
            gameController.AddRandomItem();
            soundManager.PlayAddItemSound();
        }
        else
        {
            soundManager.PlayErrorSound();
        }
    }
}
