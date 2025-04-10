using System.Collections.Generic;
using UnityEngine;

public class OrbitalAttack : IAttackBehavior
{
    private SpellData spellData;
    private FirePointManager firePointManager;
    private List<OrbitalBehavior> activeOrbs = new List<OrbitalBehavior>();
    private Transform playerTransform;

    public void Initialize(SpellData spellData, FirePointManager firePointManager)
    {
        this.spellData = spellData;
        this.firePointManager = firePointManager;
        this.playerTransform = firePointManager.transform;
        
        CreateOrbs();
    }

    private void CreateOrbs()
    {
        int numberOfOrbs = spellData.amount;
        
        // Garante que todas as orbs antigas sejam destruídas
        foreach (var orb in activeOrbs)
        {
            if (orb != null)
            {
                Object.Destroy(orb.gameObject);
            }
        }
        activeOrbs.Clear();

        // Distribui as orbs igualmente em círculo
        for (int i = 0; i < numberOfOrbs; i++)
        {
            float angle = (360f / numberOfOrbs) * i;
            OrbitalBehavior orb = CreateOrb(angle, i);
            activeOrbs.Add(orb);
            
            // Debug para verificar os ângulos
            Debug.Log($"Created orb {i} at angle {angle}");
        }
    }

    private OrbitalBehavior CreateOrb(float startAngle, int index)
    {
        GameObject orb = Object.Instantiate(spellData.projectilePrefab, playerTransform.position, Quaternion.identity);
        orb.transform.SetParent(playerTransform);
        orb.name = $"Orb_{index}"; // Facilita debug

        OrbitalBehavior orbBehavior = orb.GetComponent<OrbitalBehavior>();
        if (orbBehavior == null)
        {
            orbBehavior = orb.AddComponent<OrbitalBehavior>();
        }

        orbBehavior.Initialize(spellData, startAngle, index, spellData.amount);
        return orbBehavior;
    }

    public void Execute(Vector2 originPosition) { }

    public void UpdateAttack()
    {
        // Remove orbs nulos
        activeOrbs.RemoveAll(orb => orb == null);

        // Recria se o número estiver errado
        if (activeOrbs.Count != spellData.amount)
        {
            Debug.Log($"Recreating orbs. Current: {activeOrbs.Count}, Expected: {spellData.amount}");
            CreateOrbs();
        }
    }

    public SpellData GetSpellData()
    {
        return spellData;
    }
}