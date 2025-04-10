using UnityEngine;

public class AreaEffectBehavior : MonoBehaviour
{
    private SpellData spellData;
    private float lifetime = 0.5f;
    private float currentLifetime;
    private ElementalEffectManager effectManager;
    private SpriteRenderer spriteRenderer;
    
    // Rotação automática
    private float rotationSpeed = 180f; // Graus por segundo
    private float currentAngle;

    public void Initialize(SpellData spellData)
    {
        this.spellData = spellData;
        currentLifetime = 0;
        effectManager = FindObjectOfType<ElementalEffectManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        // Começa com um ângulo aleatório
        currentAngle = Random.Range(0f, 360f);
        transform.rotation = Quaternion.Euler(0, 0, currentAngle);

        StartCoroutine(SimpleAnimation());
    }

    private System.Collections.IEnumerator SimpleAnimation()
    {
        float startScale = 0.1f;
        float targetScale = 1f;
        Color startColor = new Color(1f, 0.3f, 0f, 0.8f);
        Color endColor = new Color(1f, 0.3f, 0f, 0f);

        transform.localScale = Vector3.one * startScale;
        spriteRenderer.color = startColor;

        while (currentLifetime < lifetime)
        {
            float t = currentLifetime / lifetime;
            
            // Escala
            float currentScale = Mathf.Lerp(startScale, targetScale, t);
            transform.localScale = new Vector3(currentScale * 2f, currentScale, 1);
            
            // Cor/Alpha
            spriteRenderer.color = Color.Lerp(startColor, endColor, t);

            yield return null;
            currentLifetime += Time.deltaTime;
        }

        Destroy(gameObject);
    }

    private void Update()
    {
        // Rotação automática
        currentAngle += rotationSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(0, 0, currentAngle);

        CheckForEnemiesInArea();
    }

    private void CheckForEnemiesInArea()
    {
        float angle = 90f; // Ângulo do cone
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, spellData.range);

        foreach (Collider2D hitCollider in hitColliders)
        {
            if (hitCollider.TryGetComponent<Enemy>(out Enemy enemy))
            {
                Vector2 directionToEnemy = (enemy.transform.position - transform.position).normalized;
                float angleToEnemy = Vector2.SignedAngle(transform.right, directionToEnemy);

                if (Mathf.Abs(angleToEnemy) <= angle / 2)
                {
                    enemy.TakeDamage(spellData.GetRandomDamage());

                    if (effectManager != null)
                    {
                        effectManager.TryApplyElementalEffect(enemy, spellData);
                    }
                }
            }
        }
    }
}