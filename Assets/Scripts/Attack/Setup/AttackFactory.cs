using UnityEngine;

public static class AttackFactory
{
    public static IAttackBehavior CreateAttack(AttackType type)
    {
        switch (type)
        {
            case AttackType.SimpleTargeted:
                return new SimpleTargetedAttack();
            case AttackType.OrbitalAttack:
                return new OrbitalAttack();
            case AttackType.AuraAttack:
                return new AuraAttack();
            default:
                Debug.LogError($"Attack type {type} not implemented!");
                return null;
        }
    }
}