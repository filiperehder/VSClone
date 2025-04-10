// Atualizar PlayerMovement.cs

using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private IInputProvider _inputProvider;
    private Rigidbody2D _rb;
    private PlayerStats _stats;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _inputProvider = GetComponent<InputManager>();
        _stats = GetComponent<PlayerStats>();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        Vector2 movement = _inputProvider.MovementInput * _stats.MovementSpeed;
        _rb.linearVelocity = movement;
    }

    public void UpdateMoveSpeed(float newSpeed)
    {
        // Método chamado quando a velocidade é atualizada via upgrade
    }
}