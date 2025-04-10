public class CharacterUpgradeReward : Reward
{
    private PlayerStats playerStats;
    private CharacterUpgradeType upgradeType;
    private float upgradeValue;

    public CharacterUpgradeReward(PlayerStats stats, CharacterUpgradeType type, float value)
    {
        this.playerStats = stats;
        this.upgradeType = type;
        this.upgradeValue = value;
        this.title = $"Upgrade {type.ToString()}";
        this.description = GetUpgradeDescription(type, value);
        // icon será definido posteriormente
    }

    public override void Apply()
    {
        switch (upgradeType)
        {
            case CharacterUpgradeType.MaxHealth:
                playerStats.UpgradeMaxHealth(upgradeValue);
                break;
            case CharacterUpgradeType.MovementSpeed:
                playerStats.UpgradeMovementSpeed(upgradeValue);
                break;
            case CharacterUpgradeType.Armor:
                playerStats.UpgradeArmor(upgradeValue);
                break;
            case CharacterUpgradeType.PickupRange:
                playerStats.UpgradePickupRange(upgradeValue);
                break;
        }
    }

    private string GetUpgradeDescription(CharacterUpgradeType type, float value)
    {
        switch (type)
        {
            case CharacterUpgradeType.MaxHealth:
                return $"Increase Max Health by {value}";
            case CharacterUpgradeType.MovementSpeed:
                return $"Increase Movement Speed by {value}";
            case CharacterUpgradeType.Armor:
                return $"Increase Armor by {value}";
            case CharacterUpgradeType.PickupRange:
                return $"Increase Pickup Range by {value}";
            default:
                return "Unknown Upgrade";
        }
    }
}