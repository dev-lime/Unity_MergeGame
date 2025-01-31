using UnityEngine;
using YG;

public class BonusLevel : BonusObject
{
    private void OnMouseDown()
    {
        YG2.saves.AddLevel();
        gameManager.PlayClickSound();
        Destroy(gameObject);
    }
}
