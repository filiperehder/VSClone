using UnityEngine;

public class SimpleTargetedAttack : IAttackBehavior
{
    private SpellData spellData;
    private FirePointManager firePointManager;
    private float cooldownTimer;
    private ElementalEffectManager effectManager;

    public void Initialize(SpellData spellData, FirePointManager firePointManager)
    {
        this.spellData = spellData;
        this.firePointManager = firePointManager;
        cooldownTimer = 0f;
        
        // Obtém a referência do ElementalEffectManager
        effectManager = Object.FindObjectOfType<ElementalEffectManager>();
        if (effectManager == null)
        {
            Debug.LogWarning("ElementalEffectManager not found in scene!");
        }
    }

    public void Execute(Vector2 originPosition)
    {
        if (cooldownTimer <= 0)
        {
            Transform target = TargetFinder.FindNearestEnemy(originPosition, spellData.range);
            
            if (target != null)
            {
                Vector2 targetPosition = target.position;
                Vector2[] firePoints = firePointManager.GetFirePoints();
                
                foreach (Vector2 firePoint in firePoints)
                {
                    FireProjectile(firePoint, targetPosition);
                }
            }
            
            cooldownTimer = spellData.cooldownTime;
        }
    }

    public void UpdateAttack()
    {
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }
    }

    public SpellData GetSpellData()
    {
        return spellData;
    }

    private void FireProjectile(Vector2 origin, Vector2 target)
    {
        if (spellData.projectilePrefab == null)
        {
            Debug.LogError($"SimpleTargetedAttack: Cannot fire {spellData.spellName} - projectile prefab is null!");
            return;
        }

        GameObject projectile = GameObject.Instantiate(spellData.projectilePrefab, origin, Quaternion.identity);
        Vector2 direction = (target - origin).normalized;
        
        Projectile projectileComponent = projectile.GetComponent<Projectile>();
        if (projectileComponent == null)
        {
            projectileComponent = projectile.AddComponent<Projectile>();
        }
        
        // Configuração do projétil com informações da spell
        projectileComponent.Initialize(direction, spellData);
        
        // Adiciona informações visuais baseadas no elemento (opcional)
        ConfigureProjectileVisuals(projectile);
    }

    private void ConfigureProjectileVisuals(GameObject projectile)
    {
        // Aqui você pode adicionar efeitos visuais baseados no elemento da spell
        // Por exemplo, mudar a cor do projétil, adicionar partículas, etc.
        switch (spellData.element)
        {
            case Element.Fire:
                // Adicionar efeitos de fogo
                break;
            case Element.Ice:
                // Adicionar efeitos de gelo
                break;
            case Element.Lightning:
                // Adicionar efeitos de raio
                break;
        }
    }
}