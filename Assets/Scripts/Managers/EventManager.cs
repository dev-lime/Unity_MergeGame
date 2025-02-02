using UnityEngine;
using System.Collections;
using System.Linq;
using YG;

public class EventManager : MonoBehaviour
{
    [System.Serializable]
    public class SpawnableObject
    {
        public GameObject prefab;
        public float spawnChance; // Шанс появления объекта
    }

    [Header("Spawner Settings")]
    public SpawnableObject[] spawnableObjects; // Массив объектов с шансами спавна

    [Header("Object Settings")]
    public Vector2 fallSpeedRange = new Vector2(1f, 5f); // Диапазон скорости падения объекта
    public Vector2 rotationSpeedRange = new Vector2(50f, 200f); // Диапазон скорости вращения

    private float spawnY;
    private Vector2 spawnRangeX;
    private Vector2 spawnIntervalRange = new Vector2(YG2.saves.GetMaxLevel() - YG2.saves.GetLevel() + 1,
                                                    (YG2.saves.GetMaxLevel() - YG2.saves.GetLevel() + 1) * 2); // Диапазон времени между спавнами

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

            // Получаем верхнюю границу экрана
            Vector3 screenTopLeft = mainCamera.ScreenToWorldPoint(new Vector3(0, Screen.height, -mainCamera.transform.position.z));
            Vector3 screenTopRight = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, -mainCamera.transform.position.z));

            // Вычисляем диапазон X для верхней границы
            spawnRangeX = new Vector2(screenTopLeft.x, screenTopRight.x);

            // Вычисляем верхнюю границу Y
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
            spawnIntervalRange = new Vector2(YG2.saves.GetMaxLevel() - YG2.saves.GetLevel() + 1,
                                            (YG2.saves.GetMaxLevel() - YG2.saves.GetLevel() + 1) * 2);

            float waitTime = Random.Range(spawnIntervalRange.x, spawnIntervalRange.y);
            yield return new WaitForSeconds(waitTime);

            SpawnObject();
        }
    }

    void SpawnObject()
    {
        float spawnX = Random.Range(spawnRangeX.x, spawnRangeX.y);
        Vector3 spawnPosition = new(spawnX, spawnY, -2f);

        GameObject selectedPrefab = GetRandomObjectByChance();
        if (selectedPrefab == null)
        {
            Debug.LogWarning("No object selected for spawning.");
            return;
        }

        GameObject newObject = Instantiate(selectedPrefab, spawnPosition, Quaternion.identity);
        BonusObject fallingObject = newObject.AddComponent<BonusObject>();
        float randomFallSpeed = Random.Range(fallSpeedRange.x, fallSpeedRange.y);
        float randomRotationSpeed = Random.Range(rotationSpeedRange.x, rotationSpeedRange.y);
        bool randomDirection = Random.value > 0.5f;
        fallingObject.Initialize(randomFallSpeed, randomRotationSpeed, randomDirection);
    }

    GameObject GetRandomObjectByChance()
    {
        float totalWeight = spawnableObjects.Sum(obj => obj.spawnChance);
        float randomPoint = Random.Range(0f, totalWeight);
        float currentWeight = 0f;

        foreach (var obj in spawnableObjects)
        {
            currentWeight += obj.spawnChance;
            if (randomPoint <= currentWeight)
            {
                return obj.prefab;
            }
        }

        return null;
    }
}
