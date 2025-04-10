using System.Collections.Generic;
using UnityEngine;

public class AuraBehavior : MonoBehaviour
{
    private SpellData spellData;
    private ElementalEffectManager effectManager;
    private float damageTickTimer;
    private float damageTickRate = 1f; // Dano por segundo
    private HashSet<Enemy> enemiesInRange = new HashSet<Enemy>();

    [SerializeField] private SpriteRenderer auraRenderer;

    public void Initialize(SpellData spellData)
    {
        this.spellData = spellData;
        effectManager = FindObjectOfType<ElementalEffectManager>();
        
        // Configura o visual da aura
        if (auraRenderer != null)
        {
            auraRenderer.color = new Color(0.7f, 0.9f, 1f, 0.3f); // Azul claro transparente
            transform.localScale = Vector3.one * spellData.range;
        }
    }

    private void Update()
    {
        damageTickTimer += Time.deltaTime;

        if (damageTickTimer >= damageTickRate)
        {
            damageTickTimer = 0f;
            DamageEnemiesInRange();
        }
    }

    private void DamageEnemiesInRange()
    {
        foreach (Enemy enemy in enemiesInRange)
        {
            if (enemy != null) // Verifica se o inimigo ainda existe
            {
                // Aplica dano reduzido
                float damage = spellData.GetRandomDamage() * 0.5f; // 50% do dano normal
                enemy.TakeDamage(damage);

                // Sempre tenta aplicar o efeito de gelo (100% de chance)
                if (effectManager != null)
                {
                    effectManager.TryApplyElementalEffect(enemy, spellData);
                }
            }
        }
        
        // Limpa inimigos nulos da lista
        enemiesInRange.RemoveWhere(enemy => enemy == null);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<Enemy>(out Enemy enemy))
        {
            enemiesInRange.Add(enemy);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent<Enemy>(out Enemy enemy))
        {
            enemiesInRange.Remove(enemy);
        }
    }
}