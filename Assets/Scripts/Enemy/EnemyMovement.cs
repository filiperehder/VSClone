using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private float _speed;
    private float _baseSpeed;
    private Transform _playerTransform;
    private Rigidbody2D _rb;
    private SpriteRenderer _spriteRenderer;

    public float MovementSpeed => _speed;

    public void Initialize(float moveSpeed, Transform target)
    {
        _baseSpeed = moveSpeed;
        _speed = moveSpeed;
        _playerTransform = target;
        
        _rb = GetComponent<Rigidbody2D>();
        if (_rb == null)
        {
            _rb = gameObject.AddComponent<Rigidbody2D>();
            _rb.gravityScale = 0f;
            _rb.freezeRotation = true;
            _rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        }

        _spriteRenderer = GetComponent<SpriteRenderer>();
        if (_spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer não encontrado no inimigo!");
        }
    }

    public void ModifySpeed(float modifier)
    {
        _speed = _baseSpeed * modifier;
    }

    private void FixedUpdate()
    {
        if (_playerTransform == null) return;

        Vector2 direction = ((Vector2)_playerTransform.position - _rb.position).normalized;
        Vector2 movement = direction * _speed;
        
        _rb.linearVelocity = movement;

        // Ao invés de rotacionar, apenas faz o flip do sprite baseado na direção
        if (_spriteRenderer != null)
        {
            // Se o inimigo está se movendo para a esquerda
            if (direction.x < 0)
            {
                _spriteRenderer.flipX = true;
            }
            // Se o inimigo está se movendo para a direita
            else if (direction.x > 0)
            {
                _spriteRenderer.flipX = false;
            }
        }
    }
}