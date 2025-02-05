using UnityEngine;

public class BonusObject : MonoBehaviour
{
    protected GameManager gameManager;
    protected SoundManager soundManager;

    private float fallSpeed;
    private float rotationSpeed;
    private bool rotateClockwise;
    private float screenBottomY; // Нижняя граница экрана

    public void Initialize(float fallSpeed, float rotationSpeed, bool rotateClockwise)
    {
        this.fallSpeed = fallSpeed;
        this.rotationSpeed = rotationSpeed;
        this.rotateClockwise = rotateClockwise;
    }

    private void Start()
    {
        gameManager = GameManager.Instance;
        soundManager = SoundManager.Instance;

        // Определение нижней границы экрана
        /*Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            print("Orthographic Size: " + mainCamera.orthographicSize);
            screenBottomY = -1 * mainCamera.orthographicSize;
            print("Screen Bottom Y: " + screenBottomY);
        }
        else
        {
            Debug.LogError("Камера не найдена! Убедитесь, что у вас есть Main Camera в сцене.");
        }*/
    }

    void Update()
    {
        // Падение вниз
        transform.position += fallSpeed * Time.deltaTime * Vector3.down;

        // Проверка, если объект опустился ниже нижней границы экрана
        /*if (transform.position.y < screenBottomY)
        {
            print("Trans: " + transform.position.y + "\tscreenBottomY: " + screenBottomY);
            Destroy(gameObject);
        }*/
        if (transform.position.y < -20)
        {
            print("Trans: " + transform.position.y + "\tscreenBottomY: " + screenBottomY);
            Destroy(gameObject);
        }

        // Вращение вокруг оси Z
        float direction = rotateClockwise ? 1f : -1f;
        transform.Rotate(0f, 0f, direction * rotationSpeed * Time.deltaTime);
    }
}
