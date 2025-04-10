// RewardManager.cs
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

public class RewardManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private WaveManager waveManager;
    [SerializeField] private SpellInventory spellInventory;
    [SerializeField] private PlayerStats playerStats;

    [Header("Available Spells")]
    [SerializeField] private List<SpellData> availableNewSpells;

    [Header("Reward Settings")]
    [SerializeField] private int rewardsPerWave = 3;

    [Header("Character Upgrade Values")]
    [SerializeField] private float healthUpgradeAmount = 10f;
    [SerializeField] private float moveSpeedUpgradeAmount = 0.5f;
    [SerializeField] private float armorUpgradeAmount = 5f;
    [SerializeField] private float pickupRangeUpgradeAmount = 0.5f;

    public UnityEvent<List<Reward>> onRewardsGenerated;

    private void Start()
    {
        if (waveManager != null)
        {
            waveManager.onWaveComplete.AddListener(HandleWaveCompleted);
        }
        else
        {
            Debug.LogError("WaveManager reference is missing!");
        }

        // Verificar todas as referências necessárias
        if (spellInventory == null)
            Debug.LogError("SpellInventory reference is missing!");
        if (playerStats == null)
            Debug.LogError("PlayerStats reference is missing!");
        if (availableNewSpells == null || availableNewSpells.Count == 0)
            Debug.LogWarning("No available spells configured!");
    }
    public void SimulateRewardChoice(int rewardIndex)
    {
        List<Reward> rewards = GenerateRewards();
        if (rewardIndex >= 0 && rewardIndex < rewards.Count)
        {
            Reward chosenReward = rewards[rewardIndex];
            Debug.Log($"=== Applying Reward ===");
            Debug.Log($"Chosen: {chosenReward.title}");
            ApplyReward(chosenReward);
            Debug.Log("Reward applied successfully!");

            // Notifica o WaveManager que uma recompensa foi selecionada
            waveManager.OnRewardSelected();
        }
        else
        {
            Debug.LogWarning($"Invalid reward index: {rewardIndex}");
        }
    }

    // Para teste via botão ou tecla
    private void Update()
    {
        // Pressione 1, 2 ou 3 para simular a escolha de recompensa
        if (Input.GetKeyDown(KeyCode.Alpha1)) SimulateRewardChoice(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SimulateRewardChoice(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SimulateRewardChoice(2);
    }

    private void HandleWaveCompleted()
    {
        List<Reward> rewards = GenerateRewards();
        LogRewards(rewards);
        onRewardsGenerated?.Invoke(rewards);
    }

    private List<Reward> GenerateRewards()
    {
        List<Reward> possibleRewards = new List<Reward>();

        // Adiciona possíveis upgrades de spell
        AddPossibleSpellUpgrades(possibleRewards);

        // Adiciona possíveis novas spells
        AddPossibleNewSpells(possibleRewards);

        // Adiciona possíveis upgrades de personagem
        AddPossibleCharacterUpgrades(possibleRewards);

        // Seleciona rewardsPerWave recompensas aleatórias
        List<Reward> selectedRewards = new List<Reward>();
        while (selectedRewards.Count < rewardsPerWave && possibleRewards.Count > 0)
        {
            int randomIndex = Random.Range(0, possibleRewards.Count);
            selectedRewards.Add(possibleRewards[randomIndex]);
            possibleRewards.RemoveAt(randomIndex);
        }

        return selectedRewards;
    }

    private void AddPossibleSpellUpgrades(List<Reward> rewards)
    {
        // Verificação de null para spellInventory
        if (spellInventory == null)
        {
            Debug.LogError("SpellInventory reference is missing in RewardManager!");
            return;
        }

        List<SpellData> upgradeableSpells = spellInventory.GetUpgradeableSpells();
    
        // Verificação de null para upgradeableSpells
        if (upgradeableSpells == null)
        {
            Debug.LogError("GetUpgradeableSpells returned null!");
            return;
        }

        foreach (SpellData spell in upgradeableSpells)
        {
            // Verificação de null para spell e suas upgrades
            if (spell == null)
            {
                Debug.LogWarning("Null spell found in upgradeableSpells!");
                continue;
            }

            if (spell.availableUpgrades == null)
            {
                Debug.LogWarning($"availableUpgrades is null for spell: {spell.name}");
                continue;
            }

            foreach (SpellUpgrade upgrade in spell.availableUpgrades)
            {
                if (upgrade == null)
                {
                    Debug.LogWarning($"Null upgrade found in spell: {spell.name}");
                    continue;
                }

                rewards.Add(new SpellUpgradeReward(spell, upgrade));
            }
        }
    }

    private void AddPossibleNewSpells(List<Reward> rewards)
    {
        if (!spellInventory.CanAddNewSpell()) return;

        foreach (SpellData spell in availableNewSpells)
        {
            if (!spellInventory.unlockedSpells.Contains(spell))
            {
                rewards.Add(new NewSpellReward(spell, spellInventory));
            }
        }
    }

    private void AddPossibleCharacterUpgrades(List<Reward> rewards)
    {
        rewards.Add(new CharacterUpgradeReward(playerStats, CharacterUpgradeType.MaxHealth, healthUpgradeAmount));
        rewards.Add(new CharacterUpgradeReward(playerStats, CharacterUpgradeType.MovementSpeed, moveSpeedUpgradeAmount));
        rewards.Add(new CharacterUpgradeReward(playerStats, CharacterUpgradeType.Armor, armorUpgradeAmount));
        rewards.Add(new CharacterUpgradeReward(playerStats, CharacterUpgradeType.PickupRange, pickupRangeUpgradeAmount));
    }

    public void ApplyReward(Reward reward)
    {
        reward.Apply();
    }

    // RewardManager.cs - Adicione este método
    private void LogRewards(List<Reward> rewards)
    {
        Debug.Log("=== Wave Completed - Available Rewards ===");
        for (int i = 0; i < rewards.Count; i++)
        {
            Reward reward = rewards[i];
            Debug.Log($"Reward {i + 1}:");
            Debug.Log($"- Title: {reward.title}");
            Debug.Log("-------------------");
        }
    }
}