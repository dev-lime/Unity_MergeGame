using YG;

public class BonusAdv : BonusObject
{
    private void OnMouseDown()
    {
        YG2.RewardedAdvShow("coins", gameManager.GetReward);
        gameManager.PlayClickSound();
        Destroy(gameObject);
    }
}
