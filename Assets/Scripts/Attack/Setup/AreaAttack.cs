using UnityEngine;

public class AreaAttack : IAttackBehavior
{
    private SpellData spellData;
    private FirePointManager firePointManager;
    private float cooldownTimer;

    public void Initialize(SpellData spellData, FirePointManager firePointManager)
    {
        this.spellData = spellData;
        this.firePointManager = firePointManager;
        cooldownTimer = 0f;
    }

    public void Execute(Vector2 originPosition)
    {
        if (cooldownTimer <= 0)
        {
            Vector2[] firePoints = firePointManager.GetFirePoints();
            
            foreach (Vector2 firePoint in firePoints)
            {
                CreateFireWave(firePoint);
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

    private void CreateFireWave(Vector2 origin)
    {
        if (spellData.projectilePrefab == null)
        {
            Debug.LogError($"AreaAttack: Cannot create {spellData.spellName} - prefab is null!");
            return;
        }

        GameObject fireWave = Object.Instantiate(spellData.projectilePrefab, origin, Quaternion.identity);
        AreaEffectBehavior areaEffect = fireWave.GetComponent<AreaEffectBehavior>();
        
        if (areaEffect != null)
        {
            areaEffect.Initialize(spellData);
        }
    }
}