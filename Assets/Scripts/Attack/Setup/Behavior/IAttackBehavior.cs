using UnityEngine;

public interface IAttackBehavior
{
    void Initialize(SpellData spellData, FirePointManager firePointManager);
    void Execute(Vector2 originPosition);
    void UpdateAttack();
    SpellData GetSpellData();
}