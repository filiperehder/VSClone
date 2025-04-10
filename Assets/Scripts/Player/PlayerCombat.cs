using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("Combat Settings")]
    [SerializeField] private float timeBetweenSpells = 0.5f; // Cooldown entre diferentes spells

    private SpellInventory _spellInventory;
    private List<IAttackBehavior> _activeAttacks = new List<IAttackBehavior>();
    private FirePointManager _firePointManager;
    private ElementalEffectManager _effectManager;
    private PlayerAnimationController _animationController;
    
    private int _currentSpellIndex = 0;
    private float _spellSwitchTimer = 0f;

    private void Awake()
    {
        _animationController = GetComponent<PlayerAnimationController>();
        if (_animationController == null)
        {
            Debug.LogWarning("PlayerAnimationController não encontrado!");
        }
        
        _spellInventory = GetComponent<SpellInventory>();
        if (_spellInventory == null)
        {
            _spellInventory = gameObject.AddComponent<SpellInventory>();
        }

        _firePointManager = GetComponent<FirePointManager>();
        if (_firePointManager == null)
        {
            _firePointManager = gameObject.AddComponent<FirePointManager>();
        }
        _firePointManager.Initialize();

        _effectManager = FindObjectOfType<ElementalEffectManager>();
        if (_effectManager == null)
        {
            GameObject effectManagerObj = new GameObject("EffectManager");
            _effectManager = effectManagerObj.AddComponent<ElementalEffectManager>();
        }

        _spellInventory.OnSpellAdded += HandleNewSpellAdded;
    }

    private void Start()
    {
        StartCoroutine(InitializeStartingSpells());
    }

    private IEnumerator InitializeStartingSpells()
    {
        yield return null;

        Debug.Log($"Initializing {_spellInventory.unlockedSpells.Count} spells");
        foreach (var spell in _spellInventory.unlockedSpells)
        {
            Debug.Log($"Initializing spell: {spell.spellName}");
            InitializeAttackForSpell(spell);
        }
    }

    private void HandleNewSpellAdded(SpellData spell)
    {
        Debug.Log($"New spell added: {spell.spellName}");
        InitializeAttackForSpell(spell);
    }

    private void InitializeAttackForSpell(SpellData spell)
    {
        if (spell == null)
        {
            Debug.LogError("Attempting to initialize null spell!");
            return;
        }

        IAttackBehavior attack = AttackFactory.CreateAttack(spell.attackType);
        if (attack == null)
        {
            Debug.LogError($"Failed to create attack behavior for spell: {spell.spellName}");
            return;
        }

        attack.Initialize(spell, _firePointManager);
        _activeAttacks.Add(attack);
        Debug.Log($"Successfully initialized attack for {spell.spellName}. Total active attacks: {_activeAttacks.Count}");
    }

    private void Update()
    {
        if (_activeAttacks.Count == 0)
        {
            return;
        }

        // Atualiza todos os ataques (para manter seus cooldowns atualizados)
        foreach (var attack in _activeAttacks)
        {
            if (attack != null)
            {
                attack.UpdateAttack();
            }
        }

        // Processa o timer entre spells
        if (_spellSwitchTimer > 0)
        {
            _spellSwitchTimer -= Time.deltaTime;
            return;
        }

        // Tenta executar a spell atual
        if (_currentSpellIndex < _activeAttacks.Count)
        {
            var currentAttack = _activeAttacks[_currentSpellIndex];
            if (currentAttack != null)
            {
                currentAttack.Execute(transform.position);
                // Dispara a animação de ataque
                if (_animationController != null)
                {
                    _animationController.TriggerAttack();
                }
            }

            // Avança para a próxima spell
            _currentSpellIndex = (_currentSpellIndex + 1) % _activeAttacks.Count;
            _spellSwitchTimer = timeBetweenSpells;
        }
        else
        {
            _currentSpellIndex = 0;
        }
    }

    public void RemoveAttack(SpellData spell)
    {
        var attackToRemove = _activeAttacks.Find(attack => attack.GetSpellData() == spell);
        if (attackToRemove != null)
        {
            int removedIndex = _activeAttacks.IndexOf(attackToRemove);
            _activeAttacks.Remove(attackToRemove);
            
            // Ajusta o índice atual se necessário
            if (_currentSpellIndex > removedIndex)
            {
                _currentSpellIndex--;
            }
            if (_currentSpellIndex >= _activeAttacks.Count)
            {
                _currentSpellIndex = 0;
            }
            
            Debug.Log($"Removed attack for {spell.spellName}. Remaining attacks: {_activeAttacks.Count}");
        }
    }

    private void OnDestroy()
    {
        if (_spellInventory != null)
        {
            _spellInventory.OnSpellAdded -= HandleNewSpellAdded;
        }
    }

#if UNITY_EDITOR
    private void OnGUI()
    {
        GUILayout.Label($"Current Spell Index: {_currentSpellIndex}");
        GUILayout.Label($"Time until next spell: {_spellSwitchTimer:F2}");
        
        if (_activeAttacks.Count > 0 && _currentSpellIndex < _activeAttacks.Count)
        {
            var currentSpell = _activeAttacks[_currentSpellIndex].GetSpellData();
            if (currentSpell != null)
            {
                GUILayout.Label($"Current Spell: {currentSpell.spellName}");
            }
        }
    }
#endif
}