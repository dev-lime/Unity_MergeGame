using UnityEngine;
using YG;

public class BonusLevel : BonusObject
{
    public ParticleSystem particlePrefab;

    private void OnMouseDown()
    {
        if (YG2.saves.GetLevel() < YG2.saves.GetMaxLevel())
        {
            YG2.saves.AddLevel();
            soundManager.PlayAddLevelSound();

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
        }
        else
        {
            soundManager.PlayErrorSound();
        }
        
        Destroy(gameObject); // ”ничтожаем объект
    }
}
