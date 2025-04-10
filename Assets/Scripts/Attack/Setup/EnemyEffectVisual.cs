using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectType
{
    None,
    Freeze,
    Burn,
    Shock
}

public class EnemyEffectVisual : MonoBehaviour
{
    [System.Serializable]
    private class EffectSettings
    {
        public Color tintColor = Color.white;
        public float tintIntensity = 0.5f;
        public bool useFlashing = false;
        public float flashRate = 0.5f;
    }

    [Header("Effect Settings")]
    [SerializeField] private EffectSettings freezeEffect = new()
    {
        tintColor = new Color(0.7f, 0.9f, 1f, 1f), // Azul claro
        tintIntensity = 0.5f
    };

    [SerializeField] private EffectSettings burnEffect = new()
    {
        tintColor = new Color(1f, 0.4f, 0f, 1f), // Laranja
        tintIntensity = 0.3f,
        useFlashing = true,
        flashRate = 0.3f
    };

    [SerializeField] private EffectSettings shockEffect = new()
    {
        tintColor = new Color(1f, 1f, 0.4f, 1f), // Amarelo
        tintIntensity = 0.4f,
        useFlashing = true,
        flashRate = 0.1f
    };

    [Header("References")]
    [SerializeField] private SpriteRenderer targetRenderer;

    private Color originalColor;
    private Material materialInstance;
    private Dictionary<EffectType, Coroutine> activeEffects = new();

    private void Awake()
    {
        if (targetRenderer == null)
        {
            targetRenderer = GetComponent<SpriteRenderer>();
        }

        if (targetRenderer != null)
        {
            materialInstance = new Material(targetRenderer.material);
            targetRenderer.material = materialInstance;
            originalColor = targetRenderer.color;
        }
    }

    public void ShowEffect(EffectType effectType, bool active, float duration = 0)
    {
        if (targetRenderer == null) return;

        // Remove efeito anterior do mesmo tipo se existir
        if (activeEffects.TryGetValue(effectType, out Coroutine existingCoroutine))
        {
            StopCoroutine(existingCoroutine);
            activeEffects.Remove(effectType);
        }

        if (active)
        {
            var coroutine = StartCoroutine(ApplyEffectRoutine(effectType, duration));
            activeEffects[effectType] = coroutine;
        }
        else
        {
            UpdateVisuals();
        }
    }

    private IEnumerator ApplyEffectRoutine(EffectType effectType, float duration)
    {
        EffectSettings settings = GetSettingsForEffect(effectType);
        float endTime = Time.time + duration;

        while (Time.time < endTime)
        {
            if (settings.useFlashing)
            {
                float flashIntensity = Mathf.PingPong(Time.time / settings.flashRate, settings.tintIntensity);
                ApplyTint(settings.tintColor, flashIntensity);
            }
            else
            {
                ApplyTint(settings.tintColor, settings.tintIntensity);
            }
            yield return null;
        }

        activeEffects.Remove(effectType);
        UpdateVisuals();
    }

    private void UpdateVisuals()
    {
        if (activeEffects.Count == 0)
        {
            targetRenderer.color = originalColor;
            return;
        }

        // Combina os efeitos ativos
        Color finalColor = originalColor;
        foreach (var effectType in activeEffects.Keys)
        {
            var settings = GetSettingsForEffect(effectType);
            finalColor = Color.Lerp(finalColor, settings.tintColor, settings.tintIntensity);
        }
        targetRenderer.color = finalColor;
    }

    private void ApplyTint(Color tintColor, float intensity)
    {
        targetRenderer.color = Color.Lerp(originalColor, tintColor, intensity);
    }

    private EffectSettings GetSettingsForEffect(EffectType effectType)
    {
        return effectType switch
        {
            EffectType.Freeze => freezeEffect,
            EffectType.Burn => burnEffect,
            EffectType.Shock => shockEffect,
            _ => new EffectSettings()
        };
    }

    private void OnDestroy()
    {
        if (materialInstance != null)
        {
            Destroy(materialInstance);
        }
    }
}