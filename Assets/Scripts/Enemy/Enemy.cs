using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float movementSpeed = 3f;
    [SerializeField] private float damageOnCollision = 10f;

    private EnemyMovement _movement;
    private EnemyHealth _health;
    private EnemyStatus _status;
    private Transform _playerTransform;

    private void Awake()
    {
        _movement = GetComponent<EnemyMovement>();
        _health = GetComponent<EnemyHealth>();
        _status = GetComponent<EnemyStatus>();
    
        if (_movement == null)
            _movement = gameObject.AddComponent<EnemyMovement>();
    
        if (_health == null)
            _health = gameObject.AddComponent<EnemyHealth>();
        
        if (_status == null)
            _status = gameObject.AddComponent<EnemyStatus>();
    }

    private void Start()
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        _movement.Initialize(movementSpeed, _playerTransform);
        _health.Initialize(maxHealth);

        _health.OnEnemyDeath += () => {
            FindFirstObjectByType<WaveManager>()?.OnEnemyDefeated();
        };
    }

    public void TakeDamage(float damage)
    {
        _health.TakeDamage(damage);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // TODO: Implement player damage
            Debug.Log($"Dealt {damageOnCollision} damage to player");
        }
    }
}