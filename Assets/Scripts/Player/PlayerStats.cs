// PlayerStats.cs
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Base Stats")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth;
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float armor = 0f;
    [SerializeField] private float pickupRange = 1f;

    public float MaxHealth => maxHealth;
    public float CurrentHealth => currentHealth;
    public float MovementSpeed => movementSpeed;
    public float Armor => armor;
    public float PickupRange => pickupRange;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void UpgradeMaxHealth(float amount)
    {
        float healthPercent = currentHealth / maxHealth;
        maxHealth += amount;
        currentHealth = maxHealth * healthPercent;
    }

    public void UpgradeMovementSpeed(float amount)
    {
        movementSpeed += amount;
        // Atualiza a velocidade no PlayerMovement
        if (TryGetComponent<PlayerMovement>(out PlayerMovement movement))
        {
            movement.UpdateMoveSpeed(movementSpeed);
        }
    }

    public void UpgradeArmor(float amount)
    {
        armor += amount;
    }

    public void UpgradePickupRange(float amount)
    {
        pickupRange += amount;
    }

    public void TakeDamage(float damage)
    {
        float finalDamage = damage * (1 - (armor / 100f)); // Armor reduz a porcentagem do dano
        currentHealth = Mathf.Max(0, currentHealth - finalDamage);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Implementar lógica de morte do player
        Debug.Log("Player died!");
    }
}