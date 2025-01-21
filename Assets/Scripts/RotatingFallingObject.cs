using UnityEngine;

public class RotatingFallingObject : MonoBehaviour
{
    public GameManager gameManager;

    private float fallSpeed;
    private float rotationSpeed;
    private bool rotateClockwise;

    public void Initialize(float fallSpeed, float rotationSpeed, bool rotateClockwise)
    {
        this.fallSpeed = fallSpeed;
        this.rotationSpeed = rotationSpeed;
        this.rotateClockwise = rotateClockwise;
    }

    private void Start()
    {
        gameManager = GameManager.Instance;
    }

    void Update()
    {
        // Падение вниз
        transform.position += fallSpeed * Time.deltaTime * Vector3.down;

        // Вращение вокруг оси Z
        float direction = rotateClockwise ? 1f : -1f;
        transform.Rotate(0f, 0f, direction * rotationSpeed * Time.deltaTime);
    }
}
