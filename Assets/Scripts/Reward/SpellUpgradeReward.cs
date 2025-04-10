// SpellUpgradeReward.cs
public class SpellUpgradeReward : Reward
{
    private SpellData targetSpell;
    private SpellUpgrade upgrade;

    public SpellUpgradeReward(SpellData spell, SpellUpgrade upgrade)
    {
        this.targetSpell = spell;
        this.upgrade = upgrade;
        this.title = $"Upgrade {spell.spellName}";
        this.description = upgrade.description;
        // icon será definido posteriormente
    }

    public override void Apply()
    {
        targetSpell.ApplyUpgrade(upgrade);
    }
}