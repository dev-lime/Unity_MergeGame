using UnityEngine;

public class BonusItem : BonusObject
{
    public ParticleSystem particlePrefab;

    private void OnMouseDown()
    {
        GameController gameController = FindFirstObjectByType<GameController>();

        if (gameController.PlaceRandomItemToRandomSlot())
        {
            gameManager.PlayAddItemSound();

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
            gameManager.PlayErrorSound();
        }
        
        Destroy(gameObject); // ”ничтожаем объект
    }
}
