using UnityEngine;
using YG;

public class BonusAdv : BonusObject
{
    public ParticleSystem particlePrefab;

    private void OnMouseDown()
    {
        YG2.RewardedAdvShow("coins", gameManager.GetReward);
        soundManager.PlayClickSound();

        ParticleSystem spawnedParticles = Instantiate(particlePrefab, transform.position, Quaternion.identity);
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null && spriteRenderer.sprite != null)
        {
            Texture2D texture = spriteRenderer.sprite.texture;
            var renderer = spawnedParticles.GetComponent<ParticleSystemRenderer>();

            if (renderer != null && texture != null)
            {
                renderer.material = new Material(Shader.Find("Sprites/Default"));
                renderer.material.mainTexture = texture;
            }
        }
        else
        {
            Debug.LogWarning("SpriteRenderer ����������� ��� � ������� ��� �������!", this);
        }

        spawnedParticles.Play();
        Destroy(gameObject); // ���������� ������
    }
}
