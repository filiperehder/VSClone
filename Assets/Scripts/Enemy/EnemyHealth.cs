using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    private float _currentHealth;
    private float _maximumHealth;
    private DamageFlash _damageFlash;
    public System.Action OnEnemyDeath;

    public void Initialize(float maxHealth)
    {
        _maximumHealth = maxHealth;
        _currentHealth = maxHealth;
        _damageFlash = GetComponent<DamageFlash>();
    }

    public void TakeDamage(float damage)
    {
        _currentHealth -= damage;
        
        // Determina se é um golpe crítico (exemplo: 20% de chance)
        bool isCritical = Random.value < 0.2f;
        
        // Mostra o texto de dano usando a pool
        DamageTextPool.Instance.ShowDamageText(damage, transform.position, isCritical);
        
        if (_damageFlash != null)
        {
            _damageFlash.Flash();
        }

        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        OnEnemyDeath?.Invoke();
        Destroy(gameObject);
    }

    public float HealthPercent => _currentHealth / _maximumHealth;
}