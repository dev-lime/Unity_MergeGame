using UnityEngine;

public class ParticleSelfDestruct : MonoBehaviour
{
    private ParticleSystem ps;

    void Start()
    {
        // �������� ��������� ParticleSystem
        ps = GetComponent<ParticleSystem>();

        // ���� ��������� �� ������, ������� ��������������
        if (ps == null)
        {
            Debug.LogWarning("ParticleSystem component not found!");
        }
    }

    void Update()
    {
        // ���� ������� ������ ���������� � �� ���������������, ���������� ������
        if (ps != null && !ps.IsAlive())
        {
            Destroy(gameObject);
        }
    }
}
