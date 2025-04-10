using UnityEngine;

public class AuraAttack : IAttackBehavior
{
    private SpellData spellData;
    private FirePointManager firePointManager;
    private GameObject auraInstance;

    public void Initialize(SpellData spellData, FirePointManager firePointManager)
    {
        this.spellData = spellData;
        this.firePointManager = firePointManager;
        
        CreateAura();
    }

    private void CreateAura()
    {
        if (auraInstance != null)
        {
            Object.Destroy(auraInstance);
        }

        auraInstance = Object.Instantiate(spellData.projectilePrefab, 
            firePointManager.transform.position, 
            Quaternion.identity);
        auraInstance.transform.SetParent(firePointManager.transform);

        AuraBehavior auraBehavior = auraInstance.GetComponent<AuraBehavior>();
        if (auraBehavior == null)
        {
            auraBehavior = auraInstance.AddComponent<AuraBehavior>();
        }

        auraBehavior.Initialize(spellData);
    }

    public void Execute(Vector2 originPosition)
    {
        // A aura é contínua, não precisa executar nada
    }

    public void UpdateAttack()
    {
        // A aura se atualiza automaticamente
    }

    public SpellData GetSpellData()
    {
        return spellData;
    }
}