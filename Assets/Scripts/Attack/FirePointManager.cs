// Classe para gerenciar os FirePoints

using System.Linq;
using UnityEngine;

public class FirePointManager : MonoBehaviour
{
    [SerializeField] private Transform[] firePoints;
    [SerializeField] private FirePointPattern pattern;
    
    private int currentFirePointIndex;
    
    public void Initialize()
    {
        currentFirePointIndex = 0;
        
        // Validação para garantir que temos pelo menos um FirePoint
        if (firePoints == null || firePoints.Length == 0)
        {
            Debug.LogWarning("No FirePoints assigned! Creating a default one.");
            CreateDefaultFirePoint();
        }
    }

    private void CreateDefaultFirePoint()
    {
        GameObject defaultPoint = new GameObject("DefaultFirePoint");
        defaultPoint.transform.SetParent(transform);
        defaultPoint.transform.localPosition = Vector3.zero;
        firePoints = new Transform[] { defaultPoint.transform };
    }

    public Vector2[] GetFirePoints(bool advance = true)
    {
        switch (pattern)
        {
            case FirePointPattern.Single:
                return new Vector2[] { firePoints[0].position };
                
            case FirePointPattern.AllSimultaneous:
                return firePoints.Select(fp => (Vector2)fp.position).ToArray();
                
            case FirePointPattern.Sequential:
            case FirePointPattern.Alternating:
                Vector2[] points = new Vector2[] { firePoints[currentFirePointIndex].position };
                if (advance)
                {
                    AdvanceFirePointIndex();
                }
                return points;
                
            default:
                return new Vector2[] { firePoints[0].position };
        }
    }

    private void AdvanceFirePointIndex()
    {
        currentFirePointIndex = (currentFirePointIndex + 1) % firePoints.Length;
    }

    // Método para visualização dos FirePoints no editor
    private void OnDrawGizmos()
    {
        if (firePoints == null) return;

        foreach (Transform firePoint in firePoints)
        {
            if (firePoint == null) continue;
            
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(firePoint.position, 0.1f);
            Gizmos.DrawLine(transform.position, firePoint.position);
        }
    }
}