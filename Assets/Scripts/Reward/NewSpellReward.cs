public class NewSpellReward : Reward
{
    private SpellData newSpell;
    private SpellInventory inventory;

    public NewSpellReward(SpellData spell, SpellInventory inventory)
    {
        this.newSpell = spell;
        this.inventory = inventory;
        this.title = $"New Spell: {spell.spellName}";
        this.description = $"Learn {spell.spellName}";
        // icon será definido posteriormente
    }

    public override void Apply()
    {
        inventory.AddSpell(newSpell);
    }
}