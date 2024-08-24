using UnityEngine;
using System.Collections.Generic;

public static class Utilities
{
    public static Vector3 GetRandomPoint(float goalBias, Transform targetPosition)
    {
        if (Random.value < goalBias)
        {
            return targetPosition.position;
        }
        else
        {
            float x = Random.Range(-50, 50); // Adjust based on your environment size
            float y = Random.Range(-50, 50);
            float z = Random.Range(-50, 50);
            return new Vector3(x, y, z);
        }
    }

    public static Node GetNearestNode(List<Node> nodes, Vector3 point)
    {
        Node nearestNode = null;
        float minDist = float.MaxValue;
        foreach (Node node in nodes)
        {
            float dist = Vector3.Distance(node.Position, point);
            if (dist < minDist)
            {
                minDist = dist;
                nearestNode = node;
            }
        }
        return nearestNode;
    }

    public static Vector3 Steer(Vector3 from, Vector3 to, float stepSize)
    {
        Vector3 direction = (to - from).normalized;
        return from + direction * stepSize;
    }

    public static bool IsCollisionFree(Vector3 from, Vector3 to, Transform targetPosition, float safeDistance)
    {
        Vector3 direction = to - from;
        float distance = direction.magnitude;
        RaycastHit hit;
        if (Physics.Raycast(from, direction, out hit, distance))
        {
            if (hit.collider.gameObject != targetPosition.gameObject && hit.distance < safeDistance)
            {
                return false;
            }
        }
        return true;
    }

    public static List<Vector3> ExtractPath(Node endNode)
    {
        List<Vector3> path = new List<Vector3>();
        Node currentNode = endNode;
        while (currentNode != null)
        {
            path.Add(currentNode.Position);
            currentNode = currentNode.Parent;
        }
        path.Reverse();

        // Visualize path
        for (int i = 0; i < path.Count - 1; i++)
        {
            Debug.DrawLine(path[i], path[i + 1], Color.red, 20f);
        }

        return path;
    }

    public static float CalculatePathLength(List<Vector3> path)
    {
        float totalDistance = 0.0f;

        for (int i = 0; i < path.Count - 1; i++)
        {
            totalDistance += Vector3.Distance(path[i], path[i + 1]);
        }

        return totalDistance;
    }

    // Lissage par interpolation de Catmull-Rom
    public static List<Vector3> SmoothPath(List<Vector3> path, int interpolationPoints = 10)
    {
        if (path == null || path.Count < 2) return path;

        List<Vector3> smoothedPath = new List<Vector3>();

        for (int i = 0; i < path.Count - 1; i++)
        {
            Vector3 p0 = i > 0 ? path[i - 1] : path[i];
            Vector3 p1 = path[i];
            Vector3 p2 = path[i + 1];
            Vector3 p3 = (i + 2 < path.Count) ? path[i + 2] : path[i + 1];

            for (int j = 0; j < interpolationPoints; j++)
            {
                float t = j / (float)interpolationPoints;
                smoothedPath.Add(CatmullRom(p0, p1, p2, p3, t));
            }
        }

        smoothedPath.Add(path[path.Count - 1]); // Ajouter le dernier point

        return smoothedPath;
    }

    // Méthode Catmull-Rom pour calculer un point lissé
    private static Vector3 CatmullRom(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        float t2 = t * t;
        float t3 = t2 * t;

        return 0.5f * (
            (2.0f * p1) +
            (-p0 + p2) * t +
            (2.0f * p0 - 5.0f * p1 + 4.0f * p2 - p3) * t2 +
            (-p0 + 3.0f * p1 - 3.0f * p2 + p3) * t3
        );
    }
}