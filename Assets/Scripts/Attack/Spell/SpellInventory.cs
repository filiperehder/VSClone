using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpellInventory : MonoBehaviour
{
    [SerializeField] private int maxSpells = 6;
    public List<SpellData> unlockedSpells = new List<SpellData>();

    public bool CanAddNewSpell()
    {
        return unlockedSpells.Count < maxSpells;
    }

    public void AddSpell(SpellData spell)
    {
        if (CanAddNewSpell() && !unlockedSpells.Contains(spell))
        {
            unlockedSpells.Add(spell);
            OnSpellAdded?.Invoke(spell);
        }
    }

    public SpellData GetRandomUpgradeableSpell()
    {
        var upgradeableSpells = unlockedSpells.Where(spell => spell.currentLevel < spell.maxLevel).ToList();
        
        if (upgradeableSpells.Count == 0)
            return null;
            
        return upgradeableSpells[Random.Range(0, upgradeableSpells.Count)];
    }

    public List<SpellData> GetUpgradeableSpells()
    {
        // Garantir que retorna uma lista vazia em vez de null
        if (unlockedSpells == null) return new List<SpellData>();
    
        return unlockedSpells.Where(spell => 
            spell != null && 
            spell.availableUpgrades != null && 
            spell.availableUpgrades.Count > 0
        ).ToList();
    }

    public List<SpellData> GetMaxLevelSpells()
    {
        return unlockedSpells.Where(spell => spell.currentLevel >= spell.maxLevel).ToList();
    }

    // Event para notificar quando uma nova spell é adicionada
    public delegate void SpellAddedHandler(SpellData spell);
    public event SpellAddedHandler OnSpellAdded;
}