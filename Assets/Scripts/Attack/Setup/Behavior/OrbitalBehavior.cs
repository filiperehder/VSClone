using UnityEngine;

public class OrbitalBehavior : MonoBehaviour
{
    private SpellData spellData;
    private float currentAngle;
    private float orbitSpeed = 120f; // graus por segundo
    private float orbRadius = 2f;
    private bool isActive = true;
    private float respawnTimer;
    private float respawnTime = 1f;
    private SpriteRenderer spriteRenderer;
    private ElementalEffectManager effectManager;
    
    private int orbIndex;
    private int totalOrbs;
    private float baseAngle;

    public void Initialize(SpellData spellData, float startAngle, int index, int total)
    {
        this.spellData = spellData;
        orbIndex = index;
        totalOrbs = total;
        // Calcula o ângulo base para distribuir as orbs uniformemente em círculo
        baseAngle = (360f / total) * index;
        currentAngle = baseAngle;
        
        spriteRenderer = GetComponent<SpriteRenderer>();
        effectManager = FindObjectOfType<ElementalEffectManager>();
        
        UpdatePosition();
        Debug.Log($"Orb {index}/{total} initialized at angle {baseAngle}");
    }

    private void Update()
    {
        if (!isActive)
        {
            respawnTimer -= Time.deltaTime;
            if (respawnTimer <= 0)
            {
                Respawn();
            }
            return;
        }
        
        // Atualiza o ângulo usando deltaTime para movimento suave e independente
        currentAngle += orbitSpeed * Time.deltaTime;
        
        // Mantém o ângulo entre 0 e 360 graus
        if (currentAngle >= 360f)
        {
            currentAngle -= 360f;
        }
        
        UpdatePosition();
    }

    private void UpdatePosition()
    {
        float rad = currentAngle * Mathf.Deg2Rad;
        float x = Mathf.Cos(rad) * orbRadius;
        float y = Mathf.Sin(rad) * orbRadius;
        
        transform.localPosition = new Vector3(x, y, 0);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isActive) return;

        if (other.TryGetComponent<Enemy>(out Enemy enemy))
        {
            enemy.TakeDamage(spellData.GetRandomDamage());

            if (effectManager != null)
            {
                effectManager.TryApplyElementalEffect(enemy, spellData);
            }

            Deactivate();
        }
    }

    private void Deactivate()
    {
        isActive = false;
        spriteRenderer.enabled = false;
        respawnTimer = respawnTime;
    }

    private void Respawn()
    {
        isActive = true;
        spriteRenderer.enabled = true;
        // Mantém o ângulo base original ao respawnar
        currentAngle = baseAngle;
        UpdatePosition();
    }

    private void OnDrawGizmos()
    {
        // Desenha uma linha do centro até a orb para debug visual
        if (Application.isPlaying)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.parent.position, transform.position);
        }
    }
}