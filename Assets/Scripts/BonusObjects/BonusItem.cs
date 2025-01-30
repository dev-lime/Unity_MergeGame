public class BonusItem : BonusObject
{
    private void OnMouseDown()
    {
        GameController gameController = FindFirstObjectByType<GameController>();
        gameController.PlaceRandomItemToRandomSlot();
        gameManager.PlayClickSound();
        Destroy(gameObject);
    }
}
