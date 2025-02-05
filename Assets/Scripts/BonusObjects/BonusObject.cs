using UnityEngine;

public class BonusObject : MonoBehaviour
{
    protected GameManager gameManager;
    protected SoundManager soundManager;

    private float fallSpeed;
    private float rotationSpeed;
    private bool rotateClockwise;
    private float screenBottomY; // ������ ������� ������

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

        // ����������� ������ ������� ������
        /*Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            print("Orthographic Size: " + mainCamera.orthographicSize);
            screenBottomY = -1 * mainCamera.orthographicSize;
            print("Screen Bottom Y: " + screenBottomY);
        }
        else
        {
            Debug.LogError("������ �� �������! ���������, ��� � ��� ���� Main Camera � �����.");
        }*/
    }

    void Update()
    {
        // ������� ����
        transform.position += fallSpeed * Time.deltaTime * Vector3.down;

        // ��������, ���� ������ ��������� ���� ������ ������� ������
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

        // �������� ������ ��� Z
        float direction = rotateClockwise ? 1f : -1f;
        transform.Rotate(0f, 0f, direction * rotationSpeed * Time.deltaTime);
    }
}
