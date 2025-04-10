using UnityEngine;
using System.Collections.Generic;

public class DamageTextPool : MonoBehaviour
{
    private static DamageTextPool instance;
    public static DamageTextPool Instance => instance;

    [SerializeField] private GameObject damageTextPrefab;
    [SerializeField] private int initialPoolSize = 20;
    [SerializeField] private int maxPoolSize = 100;
    [SerializeField] private Transform poolContainer;

    private Queue<DamageText> pool;
    private List<DamageText> activeTexts;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            Initialize();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Initialize()
    {
        pool = new Queue<DamageText>();
        activeTexts = new List<DamageText>();

        if (poolContainer == null)
        {
            poolContainer = new GameObject("DamageTextPool").transform;
            poolContainer.SetParent(transform);
        }

        // Cria o pool inicial
        for (int i = 0; i < initialPoolSize; i++)
        {
            CreateNewTextObject();
        }
    }

    private void CreateNewTextObject()
    {
        GameObject instance = Instantiate(damageTextPrefab, poolContainer);
        DamageText damageText = instance.GetComponent<DamageText>();
        instance.SetActive(false);
        pool.Enqueue(damageText);
    }

    public void ShowDamageText(float damage, Vector3 position, bool isCritical = false)
    {
        DamageText damageText = GetFromPool();
        if (damageText != null)
        {
            damageText.Show(damage, position, isCritical);
            activeTexts.Add(damageText);
        }
    }

    private DamageText GetFromPool()
    {
        // Tenta pegar um objeto existente da pool
        if (pool.Count > 0)
        {
            return pool.Dequeue();
        }

        // Se a pool está vazia mas não atingiu o limite máximo, cria um novo
        if (activeTexts.Count + pool.Count < maxPoolSize)
        {
            CreateNewTextObject();
            return pool.Dequeue();
        }

        // Se atingiu o limite, reutiliza o texto ativo mais antigo
        if (activeTexts.Count > 0)
        {
            DamageText oldestText = activeTexts[0];
            activeTexts.RemoveAt(0);
            oldestText.Release();
            return oldestText;
        }

        Debug.LogWarning("DamageTextPool: Não foi possível obter um texto de dano!");
        return null;
    }

    public void ReturnToPool(DamageText damageText)
    {
        activeTexts.Remove(damageText);
        pool.Enqueue(damageText);
    }

    // Opcional: Método para limpar todos os textos ativos
    public void ClearAllTexts()
    {
        foreach (var text in activeTexts.ToArray())
        {
            text.Release();
        }
        activeTexts.Clear();
    }
}