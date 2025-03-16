using UnityEngine;

public class ParticleSelfDestruct : MonoBehaviour
{
    private ParticleSystem ps;

    void Start()
    {
        // Получаем компонент ParticleSystem
        ps = GetComponent<ParticleSystem>();

        // Если компонент не найден, выводим предупреждение
        if (ps == null)
        {
            Debug.LogWarning("ParticleSystem component not found!");
        }
    }

    void Update()
    {
        // Если система частиц существует и не воспроизводится, уничтожаем объект
        if (ps != null && !ps.IsAlive())
        {
            Destroy(gameObject);
        }
    }
}
