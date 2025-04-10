using UnityEngine;

public class EnemyStatus : MonoBehaviour
{
    private ElementalEffect burnEffect = new ElementalEffect();
    private ElementalEffect freezeEffect = new ElementalEffect();
    private ElementalEffect shockEffect = new ElementalEffect();
    
    private Enemy enemy;
    private float baseSpeed;
    private float currentSlowModifier = 1f;
    private EnemyEffectVisual effectVisual;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
        if (enemy != null && enemy.GetComponent<EnemyMovement>() != null)
        {
            baseSpeed = enemy.GetComponent<EnemyMovement>().MovementSpeed;
        }

        // Inicializa o sistema visual
        effectVisual = GetComponent<EnemyEffectVisual>();
        if (effectVisual == null)
        {
            effectVisual = gameObject.AddComponent<EnemyEffectVisual>();
        }
    }

    private void Update()
    {
        UpdateBurnEffect();
        UpdateFreezeEffect();
        UpdateShockEffect();
    }

    public void ApplyBurn(float damage, float duration, float tickRate)
    {
        burnEffect.duration = duration;
        burnEffect.remainingDuration = duration;
        burnEffect.tickRate = tickRate;
        burnEffect.effectPower = damage;
        burnEffect.isActive = true;
        burnEffect.nextTickTime = Time.time + tickRate;

        // Adiciona o efeito visual
        effectVisual?.ShowEffect(EffectType.Burn, true, duration);
    }

    public void ApplyFreeze(float slowPercent, float duration)
    {
        freezeEffect.duration = duration;
        freezeEffect.remainingDuration = duration;
        freezeEffect.effectPower = slowPercent;
        freezeEffect.isActive = true;

        // Aplica o slow
        if (enemy != null && enemy.GetComponent<EnemyMovement>() != null)
        {
            currentSlowModifier = 1f - slowPercent;
            enemy.GetComponent<EnemyMovement>().ModifySpeed(currentSlowModifier);
        }

        // Adiciona o efeito visual
        effectVisual?.ShowEffect(EffectType.Freeze, true, duration);
    }

    public void ApplyShock(float chainDamage, float duration)
    {
        shockEffect.duration = duration;
        shockEffect.remainingDuration = duration;
        shockEffect.effectPower = chainDamage;
        shockEffect.isActive = true;

        // Adiciona o efeito visual
        effectVisual?.ShowEffect(EffectType.Shock, true, duration);
    }

    private void UpdateBurnEffect()
    {
        if (!burnEffect.isActive) return;

        if (Time.time >= burnEffect.nextTickTime)
        {
            // Aplica dano por tick
            enemy?.TakeDamage(burnEffect.effectPower);
            burnEffect.nextTickTime = Time.time + burnEffect.tickRate;
        }

        burnEffect.remainingDuration -= Time.deltaTime;
        if (burnEffect.remainingDuration <= 0)
        {
            burnEffect.isActive = false;
            effectVisual?.ShowEffect(EffectType.Burn, false);
        }
    }

    private void UpdateFreezeEffect()
    {
        if (!freezeEffect.isActive) return;

        freezeEffect.remainingDuration -= Time.deltaTime;
        if (freezeEffect.remainingDuration <= 0)
        {
            freezeEffect.isActive = false;
            // Restaura a velocidade normal
            if (enemy != null && enemy.GetComponent<EnemyMovement>() != null)
            {
                enemy.GetComponent<EnemyMovement>().ModifySpeed(1f);
            }
            effectVisual?.ShowEffect(EffectType.Freeze, false);
        }
    }

    private void UpdateShockEffect()
    {
        if (!shockEffect.isActive) return;

        shockEffect.remainingDuration -= Time.deltaTime;
        if (shockEffect.remainingDuration <= 0)
        {
            shockEffect.isActive = false;
            effectVisual?.ShowEffect(EffectType.Shock, false);
        }
    }

    private void OnDestroy()
    {
        // Garante que os efeitos visuais são removidos quando o inimigo é destruído
        effectVisual?.ShowEffect(EffectType.Burn, false);
        effectVisual?.ShowEffect(EffectType.Freeze, false);
        effectVisual?.ShowEffect(EffectType.Shock, false);
    }
}