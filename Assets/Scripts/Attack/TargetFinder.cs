using UnityEngine;

public class TargetFinder
{
    public static Transform FindNearestEnemy(Vector2 position, float maxRange = 50f)
    {
        Transform nearestEnemy = null;
        float nearestDistance = maxRange;

        // Encontra todos os objetos com o componente Enemy
        foreach (Enemy enemy in GameObject.FindObjectsByType<Enemy>(FindObjectsSortMode.None))
        {
            float distance = Vector2.Distance(position, enemy.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestEnemy = enemy.transform;
            }
        }

        return nearestEnemy;
    }
}