using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Vector2 direction;
    private SpellData spellData;
    private float currentLifetime;
    private const float MAX_LIFETIME = 5f;
    private float calculatedDamage;
    private ElementalEffectManager effectManager;
    
    [SerializeField] private bool rotateSprite = true;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            if (spriteRenderer == null)
            {
                spriteRenderer = GetComponent<SpriteRenderer>();
            }
        }
    }

    public void Initialize(Vector2 direction, SpellData spellData)
    {
        this.direction = direction;
        this.spellData = spellData;
        
        if (this.spellData == null)
        {
            this.calculatedDamage = spellData.GetRandomDamage();
        }
        else
        {
            this.calculatedDamage = this.spellData.GetRandomDamage();
            this.effectManager = FindObjectOfType<ElementalEffectManager>();
        }

        currentLifetime = 0f;

        // Sempre rotaciona o GameObject para a direção correta do movimento
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        // Se não deve rotacionar o sprite, reseta sua rotação para 0
        if (!rotateSprite && spriteRenderer != null)
        {
            spriteRenderer.transform.localRotation = Quaternion.identity;
        }
    }

    private void Update()
    {
        // Move o projétil para frente baseado na rotação do GameObject
        transform.Translate(Vector3.right * spellData.projectileSpeed * Time.deltaTime);

        currentLifetime += Time.deltaTime;
        if (currentLifetime >= MAX_LIFETIME)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<Enemy>(out Enemy enemy))
        {
            enemy.TakeDamage(calculatedDamage);

            if (spellData != null && effectManager != null)
            {
                effectManager.TryApplyElementalEffect(enemy, spellData);
            }

            Destroy(gameObject);
        }
    }
}