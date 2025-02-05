using UnityEngine;

public class SaleArea : MonoBehaviour
{
    [SerializeField] private GameObject shadow;
    private GameController gameController;

    private void Start()
    {
        gameController = FindFirstObjectByType<GameController>();
    }

    private void Update()
    {
        if (!gameController.GetCarringItem() && shadow.activeSelf)
        {
            shadow.SetActive(false);
        }
    }

    public void OnMouseEnter()
    {
        if (gameController.GetCarringItem())
        {
            shadow.SetActive(true);
        }
    }

    public void OnMouseExit()
    {
        if (shadow.activeSelf)
        {
            shadow.SetActive(false);
        }
    }
}
