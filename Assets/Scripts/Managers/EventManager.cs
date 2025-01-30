using UnityEngine;
using System.Collections;

public class EventManager : MonoBehaviour
{
    [Header("Spawner Settings")]
    public GameObject[] objectPrefabs; // ������ �������� ��������, ������� ����� ����������
    public float spawnY = 6f; // ������ ������ ��� �������
    public Vector2 spawnIntervalRange = new Vector2(5f, 15f); // �������� ������� ����� ��������

    [Header("Object Settings")]
    public Vector2 fallSpeedRange = new Vector2(1f, 5f); // �������� �������� ������� �������
    public Vector2 rotationSpeedRange = new Vector2(50f, 200f); // �������� �������� ��������

    private Vector2 spawnRangeX;

    void Start()
    {
        CalculateSpawnRange();
        StartCoroutine(SpawnObjects());
    }

    void CalculateSpawnRange()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            Vector3 screenLeft = mainCamera.ScreenToWorldPoint(new Vector3(0, 0, mainCamera.transform.position.z * -1));
            Vector3 screenRight = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, 0, mainCamera.transform.position.z * -1));
            spawnRangeX = new Vector2(screenLeft.x, screenRight.x);

            // �������� ������� ������� ������
            Vector3 screenTopLeft = mainCamera.ScreenToWorldPoint(new Vector3(0, Screen.height, -mainCamera.transform.position.z));
            Vector3 screenTopRight = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, -mainCamera.transform.position.z));

            // ��������� �������� X ��� ������� �������
            spawnRangeX = new Vector2(screenTopLeft.x, screenTopRight.x);

            // ��������� ������� ������� Y
            spawnY += screenTopLeft.y;
        }
        else
        {
            Debug.LogError("Main camera not found. Please ensure there is a camera tagged as MainCamera.");
        }
    }

    IEnumerator SpawnObjects()
    {
        while (true)
        {
            float waitTime = Random.Range(spawnIntervalRange.x, spawnIntervalRange.y);
            yield return new WaitForSeconds(waitTime);

            SpawnObject();
        }
    }

    void SpawnObject()
    {
        float spawnX = Random.Range(spawnRangeX.x, spawnRangeX.y);
        Vector3 spawnPosition = new(spawnX, spawnY, -2f);

        // ����� ���������� ������� �� �������
        GameObject randomPrefab = objectPrefabs[Random.Range(0, objectPrefabs.Length)];

        GameObject newObject = Instantiate(randomPrefab, spawnPosition, Quaternion.identity);
        BonusObject bonusObject = newObject.AddComponent<BonusObject>();
        float randomFallSpeed = Random.Range(fallSpeedRange.x, fallSpeedRange.y);
        float randomRotationSpeed = Random.Range(rotationSpeedRange.x, rotationSpeedRange.y);
        bool randomDirection = Random.value > 0.5f;
        bonusObject.Initialize(randomFallSpeed, randomRotationSpeed, randomDirection);
    }
}
