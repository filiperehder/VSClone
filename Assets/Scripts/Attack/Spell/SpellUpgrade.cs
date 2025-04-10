using UnityEngine;

public class SpellUpgrade
{
    public string upgradeName;
    public string description;
    public SpellModifierType modifierType;
    public float modifierValue;
    public Sprite icon;

    public void ApplyTo(SpellData spell)
    {
        switch (modifierType)
        {
            case SpellModifierType.Damage:
                spell.minDamage *= (1 + modifierValue);
                spell.maxDamage *= (1 + modifierValue);
                break;

            case SpellModifierType.CooldownReduction:
                spell.cooldownTime *= (1 - modifierValue);
                break;

            case SpellModifierType.ElementalPower:
                spell.elementalEffectPower *= (1 + modifierValue);
                break;

            case SpellModifierType.Amount:
                // Arredonda para o inteiro mais próximo
                spell.amount = Mathf.RoundToInt(spell.amount * (1 + modifierValue));
                break;
        }
    }
}