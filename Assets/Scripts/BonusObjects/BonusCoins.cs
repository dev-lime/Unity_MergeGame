using UnityEngine;
using YG;

public class BonusCoins : BonusObject
{
    //public Vector2 MoneyCountRange = new(YG2.saves.GetAddItemCost() * 2, YG2.saves.GetAddItemCost() * 100);
    public ParticleSystem particlePrefab;

    private int AddMoneyCount;

    private void Start()
    {
        gameManager = GameManager.Instance;
        soundManager = SoundManager.Instance;

        AddMoneyCount = (int)Random.Range(100, YG2.saves.GetLevel() * 100);
    }

    private void OnMouseDown()
    {
        YG2.saves.AddCoins(AddMoneyCount);
        soundManager.PlayAddCoinsSound();

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
