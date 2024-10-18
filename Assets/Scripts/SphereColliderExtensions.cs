using UnityEngine;

public static class SphereColliderExtensions
{
    public static Vector3 GetRandomPosition(this SphereCollider collider, float height)
    {
        Vector3 center = collider.transform.position + collider.center;
        float radius = collider.radius;

        Vector3 randomPoint = Random.insideUnitSphere * radius;
        randomPoint.y = height;

        return center + randomPoint;
    }
}