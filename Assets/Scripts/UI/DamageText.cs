using UnityEngine;
using TMPro;
using System.Collections;

public class DamageText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textComponent;
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float fadeSpeed = 1f;
    [SerializeField] private float lifetime = 1f;
    
    private Vector3 moveVector;
    private Color textColor;
    private float currentLifetime;
    private bool isActive;
    
    private void Awake()
    {
        if (textComponent == null)
            textComponent = GetComponent<TextMeshProUGUI>();
            
        textColor = textComponent.color;
        isActive = false;
    }

    public void Show(float damage, Vector3 position, bool isCritical = false)
    {
        gameObject.SetActive(true);
        isActive = true;
        currentLifetime = lifetime;
        
        // Reseta propriedades
        transform.position = position;
        textComponent.color = textColor;
        
        // Configura o texto
        textComponent.text = damage.ToString("F0");
        
        if (isCritical)
        {
            textComponent.fontSize *= 1.5f;
            textComponent.color = Color.red;
        }
        else
        {
            textComponent.fontSize = 36f; // Tamanho padrão
            textComponent.color = textColor;
        }

        // Direção aleatória para cima
        float randomX = Random.Range(-0.5f, 0.5f);
        moveVector = new Vector3(randomX, 1f, 0f).normalized;

        StartCoroutine(AnimateText());
    }

    private IEnumerator AnimateText()
    {
        while (currentLifetime > 0 && isActive)
        {
            currentLifetime -= Time.deltaTime;
            
            // Move o texto
            transform.position += moveVector * moveSpeed * Time.deltaTime;
            
            // Fade out
            Color color = textComponent.color;
            color.a -= fadeSpeed * Time.deltaTime;
            textComponent.color = color;

            yield return null;
        }

        Release();
    }

    public void Release()
    {
        isActive = false;
        gameObject.SetActive(false);
        DamageTextPool.Instance.ReturnToPool(this);
    }
}