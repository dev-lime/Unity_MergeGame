using UnityEngine;
using UnityEngine.SceneManagement;
using YG;

public class MenuManager : MonoBehaviour
{
    public void GoToGameScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ResetSaves()
    {
        YG2.saves.ResetSaves();
    }
}
