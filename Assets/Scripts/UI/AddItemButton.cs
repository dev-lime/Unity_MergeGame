using UnityEngine;

public class AddItemButton : MonoBehaviour
{
    public GameController gameController;

    private void OnMouseDown()
    {
        gameController.AddRandomItem();
    }
}
