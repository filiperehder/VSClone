using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator _animator;
    private Rigidbody2D _rb;
    
    // Hash IDs para os parâmetros do animator (mais eficiente que strings)
    private static readonly int IsRunning = Animator.StringToHash("IsRunning");
    private static readonly int Attack = Animator.StringToHash("Attack");
    private static readonly int Hurt = Animator.StringToHash("Hurt");

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        
        if (_animator == null)
            Debug.LogError("Animator não encontrado no Player!");
    }

    private void Update()
    {
        // Atualiza animação de movimento
        bool isMoving = _rb.linearVelocity.magnitude > 0.1f;
        _animator.SetBool(IsRunning, isMoving);

        // Atualiza a direção do sprite (flip)
        if (_rb.linearVelocity.x != 0)
        {
            transform.localScale = new Vector3(
                Mathf.Sign(_rb.linearVelocity.x), 
                1, 
                1
            );
        }
    }

    // Chamado quando o player atacar
    public void TriggerAttack()
    {
        _animator.SetTrigger(Attack);
    }

    // Chamado quando o player tomar dano
    public void TriggerHurt()
    {
        _animator.SetTrigger(Hurt);
    }
}