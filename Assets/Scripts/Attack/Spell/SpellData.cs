using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Spell", menuName = "Game/Spell")]
public class SpellData : ScriptableObject
{
    [Header("Spell Identity")]
    public string spellName;
    public Element element;
    public SpellRarity rarity;
    
    [Header("Attack Configuration")]
    public AttackType attackType;
    public GameObject projectilePrefab;

    [Header("Base Stats")]
    [Tooltip("Minimum damage this spell can deal")]
    public float minDamage = 8f;
    [Tooltip("Maximum damage this spell can deal")]
    public float maxDamage = 12f;
    [Tooltip("Time in seconds between casts")]
    public float cooldownTime = 1f;
    public float projectileSpeed = 10f;
    public float range = 50f;
   
    [Header("Amount Settings")]
    [Tooltip("Number of projectiles/orbs this spell will create")]
    public int amount = 1;
    
    [Header("Additional Effect Settings")]
    [Tooltip("Multiplicador de lentidão para spells de gelo (1 = lentidão padrão)")]
    public float slowMultiplier = 1f;
    
    [Header("Elemental Effects")]
    public float elementalEffectChance = 0.3f;
    public float elementalEffectDuration = 3f;
    public float elementalEffectPower = 1f;

    [Header("Level System")]
    public int currentLevel = 1;
    public int maxLevel = 5;

    [Header("Upgrade System")]
    public List<SpellUpgrade> appliedUpgrades;
    [SerializeField]
    private List<SpellUpgrade> _availableUpgrades = new List<SpellUpgrade>();
    
    public List<SpellUpgrade> availableUpgrades
    {
        get { return _availableUpgrades ?? new List<SpellUpgrade>(); }
        set { _availableUpgrades = value; }
    }


    public void ApplyUpgrade(SpellUpgrade upgrade)
    {
        if (currentLevel >= maxLevel) return;
        
        upgrade.ApplyTo(this);
        appliedUpgrades.Add(upgrade);
        currentLevel++;
    }
    public float GetRandomDamage()
    {
        float baseDamage = Random.Range(minDamage, maxDamage);
        return baseDamage * GetElementalMultiplier();
    }

    private float GetElementalMultiplier()
    {
        return 1f + (elementalEffectPower - 1f);
    }

    protected virtual void OnValidate()
    {
        // Base stats validation
        cooldownTime = Mathf.Max(0f, cooldownTime);
        projectileSpeed = Mathf.Max(0.1f, projectileSpeed);
        range = Mathf.Max(1f, range);
        minDamage = Mathf.Max(0f, minDamage);
        maxDamage = Mathf.Max(minDamage, maxDamage);
        amount = Mathf.Max(1, amount);
        
        // Level system validation
        maxLevel = Mathf.Max(1, maxLevel);
        currentLevel = Mathf.Clamp(currentLevel, 1, maxLevel);

        // Elemental effects validation
        elementalEffectChance = Mathf.Clamp01(elementalEffectChance);
        elementalEffectDuration = Mathf.Max(0.1f, elementalEffectDuration);
        elementalEffectPower = Mathf.Max(0f, elementalEffectPower);
        
        if (_availableUpgrades == null)
        {
            _availableUpgrades = new List<SpellUpgrade>();
        }
        
        slowMultiplier = Mathf.Max(0.1f, slowMultiplier);
    }
}