using YG;

public class AdMoney : RotatingFallingObject
{
    private void Start()
    {
        gameManager = GameManager.Instance;
    }

    private void OnMouseDown()
    {
        YG2.RewardedAdvShow("coins", gameManager.GetReward);
        Destroy(gameObject);
    }
}
