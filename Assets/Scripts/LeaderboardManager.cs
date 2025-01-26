using UnityEngine;

public class LeaderboardManager : MonoBehaviour
{
    public GameObject leaderboard;

    public void OpenLeaderboard()
    {
        leaderboard.SetActive(true);
    }

    public void CloseLeaderboard()
    {
        leaderboard.SetActive(false);
    }
}
