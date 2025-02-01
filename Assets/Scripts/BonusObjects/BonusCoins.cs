using UnityEngine;
using YG;

public class BonusCoins : BonusObject
{
    public Vector2 MoneyCountRange = new(YG2.saves.GetAddItemCost() * 2, YG2.saves.GetAddItemCost() * 100);
    public ParticleSystem particlePrefab;

    private int AddMoneyCount;

    private void Start()
    {
        gameManager = GameManager.Instance;

        AddMoneyCount = (int)Random.Range(MoneyCountRange.x, MoneyCountRange.y);
    }

    private void OnMouseDown()
    {
        YG2.saves.AddCoins(AddMoneyCount);
        gameManager.PlayClickSound();

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
            Debug.LogWarning("SpriteRenderer отсутствует или у объекта нет спрайта!", this);
        }

        spawnedParticles.Play();
        Destroy(gameObject); // ”ничтожаем объект
    }
}
