using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PulsingLight2D : MonoBehaviour
{
    [Header("Light Settings")]
    [SerializeField] private Light2D light2D;
    
    [Header("Pulse Settings")]
    [SerializeField] private float minIntensity = 0.5f;
    [SerializeField] private float maxIntensity = 1.5f;
    [SerializeField] private float pulseSpeed = 1f;
    [SerializeField] private AnimationCurve pulseCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    
    [Header("Color Settings (Optional)")]
    [SerializeField] private bool enableColorPulse = false;
    [SerializeField] private Color minColor = Color.white;
    [SerializeField] private Color maxColor = Color.white;

    private float currentTime;

    private void Awake()
    {
        // Se não foi atribuído no inspector, tenta pegar o componente
        if (light2D == null)
        {
            light2D = GetComponent<Light2D>();
        }

        if (light2D == null)
        {
            Debug.LogError("Light2D component not found!");
            enabled = false;
        }
    }

    private void Update()
    {
        if (light2D == null) return;

        // Atualiza o tempo
        currentTime += Time.deltaTime * pulseSpeed;

        // Calcula o fator de interpolação usando a curva de animação
        float t = pulseCurve.Evaluate(Mathf.PingPong(currentTime, 1f));

        // Interpola a intensidade
        light2D.intensity = Mathf.Lerp(minIntensity, maxIntensity, t);

        // Interpola a cor se estiver habilitado
        if (enableColorPulse)
        {
            light2D.color = Color.Lerp(minColor, maxColor, t);
        }
    }

    // Método para resetar a pulsação
    public void ResetPulse()
    {
        currentTime = 0f;
    }

    // Métodos para controlar a pulsação em runtime
    public void SetPulseSpeed(float speed)
    {
        pulseSpeed = speed;
    }

    public void SetIntensityRange(float min, float max)
    {
        minIntensity = min;
        maxIntensity = max;
    }
}