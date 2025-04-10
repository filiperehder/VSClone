using UnityEngine;

public class ElementalEffectManager : MonoBehaviour
{
    [Header("Fire Settings")]
    [SerializeField] private float burnDamagePercent = 0.25f;
    [SerializeField] private float burnTickRate = 0.5f;
    
    [Header("Ice Settings")]
    [SerializeField] private float freezeSlowPercent = 0.5f;
    [SerializeField] private float baseSlowPercent = 0.5f;
    
    [Header("Lightning Settings")]
    [SerializeField] private float chainDamagePercent = 0.3f;
    [SerializeField] private float chainRange = 3f;
    [SerializeField] private int maxChainTargets = 3;
    public void TryApplyElementalEffect(Enemy enemy, SpellData spell)
    {
        if (Random.value > spell.elementalEffectChance) return;

        EnemyStatus status = enemy.GetComponent<EnemyStatus>();
        if (status == null) return;

        switch (spell.element)
        {
            case Element.Fire:
                ApplyFireEffect(status, spell);
                break;
            case Element.Ice:
                ApplyIceEffect(status, spell);
                break;
            case Element.Lightning:
                ApplyLightningEffect(enemy, status, spell); // Passando o enemy como parâmetro
                break;
        }
    }

    private void ApplyFireEffect(EnemyStatus status, SpellData spell)
    {
        float burnDamage = spell.GetRandomDamage() * burnDamagePercent;
        status.ApplyBurn(burnDamage, spell.elementalEffectDuration, burnTickRate);
    }

    private void ApplyIceEffect(EnemyStatus status, SpellData spell)
    {
        status.ApplyFreeze(freezeSlowPercent, spell.elementalEffectDuration);
    }

    private void ApplyLightningEffect(Enemy sourceEnemy, EnemyStatus status, SpellData spell)
    {
        float chainDamage = spell.GetRandomDamage() * chainDamagePercent;
        status.ApplyShock(chainDamage, spell.elementalEffectDuration);

        // Chain Lightning Logic
        Collider2D[] nearbyColliders = Physics2D.OverlapCircleAll(status.transform.position, chainRange);
        int chainsRemaining = maxChainTargets;
        
        foreach (Collider2D collider in nearbyColliders)
        {
            if (chainsRemaining <= 0) break;
            
            if (collider.TryGetComponent<Enemy>(out Enemy nearbyEnemy))
            {
                if (nearbyEnemy != sourceEnemy) // Comparando com o sourceEnemy
                {
                    nearbyEnemy.TakeDamage(chainDamage);
                    chainsRemaining--;
                }
            }
        }
    }
}