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
            float x = Random.Range(-50, 50); // Ajuste selon la taille de ton environnement
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
            if (hit.collider != null && hit.collider.gameObject != targetPosition?.gameObject && hit.distance < safeDistance)
            {
                return false;
            }
        }
        return true;
    }

    public static List<Vector3> ExtractPath(Node endNode, float stepSize)
    {
        List<Vector3> path = new List<Vector3>();
        Node currentNode = endNode;
        while (currentNode != null)
        {
            path.Add(currentNode.Position);
            currentNode = currentNode.Parent;
        }
        path.Reverse();

        // Éliminer les points redondants
        List<Vector3> filteredPath = new List<Vector3> { path[0] };
        for (int i = 1; i < path.Count; i++)
        {
            if (Vector3.Distance(path[i], path[i - 1]) > stepSize / 2)
            {
                filteredPath.Add(path[i]);
            }
        }

        return filteredPath;
    }

    public static float CalculatePathLength(List<Vector3> path)
    {
        if (path == null || path.Count < 2)
        {
            Debug.LogWarning("Path is null or has less than 2 points.");
            return 0.0f;
        }

        float totalDistance = 0.0f;

        for (int i = 0; i < path.Count - 1; i++)
        {
            float distance = Vector3.Distance(path[i], path[i + 1]);
            totalDistance += distance;
        }

        return totalDistance;
    }

    public static List<Vector3> SmoothPathWithBezier(List<Vector3> path, int interpolationPoints = 3)
    {
        if (path == null || path.Count < 4) return path;

        List<Vector3> smoothedPath = new List<Vector3>();

        // Lissage par courbes Bézier cubiques avec moins d'interpolation
        for (int i = 0; i < path.Count - 3; i += 3) // Lisser un segment sur trois
        {
            Vector3 p0 = path[i];
            Vector3 p1 = path[i + 1];
            Vector3 p2 = path[i + 2];
            Vector3 p3 = path[i + 3];

            for (int j = 0; j <= interpolationPoints; j++)
            {
                float t = j / (float)interpolationPoints;
                Vector3 point = Bezier(p0, p1, p2, p3, t);
                smoothedPath.Add(point);
            }
        }

        // Ajouter le dernier point du chemin
        smoothedPath.Add(path[path.Count - 1]);

        return smoothedPath;
    }

    // Calcul d'un point sur une Bézier spline cubique
    private static Vector3 Bezier(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        Vector3 p = uuu * p0; // (1-t)^3 * p0
        p += 3 * uu * t * p1; // 3 * (1-t)^2 * t * p1
        p += 3 * u * tt * p2; // 3 * (1-t) * t^2 * p2
        p += ttt * p3; // t^3 * p3

        return p;
    }

    public static List<Vector3> SimplifyPath(List<Vector3> path, float minDistance = 0.5f)
    {
        if (path == null || path.Count < 2) return path;

        List<Vector3> simplifiedPath = new List<Vector3> { path[0] };  // Ajouter le premier point

        for (int i = 1; i < path.Count; i++)
        {
            if (Vector3.Distance(simplifiedPath[simplifiedPath.Count - 1], path[i]) > minDistance)
            {
                simplifiedPath.Add(path[i]);
            }
        }

        return simplifiedPath;
    }

    public static List<Node> RewireNodes(List<Node> nodes, Node newNode, float rewireRadius)
    {
        List<Node> neighbors = new List<Node>();
        foreach (Node node in nodes)
        {
            if (node == newNode) continue;

            float distance = Vector3.Distance(node.Position, newNode.Position);
            if (distance <= rewireRadius)
            {
                neighbors.Add(node);
            }
        }

        foreach (Node neighbor in neighbors)
        {
            float newCost = Vector3.Distance(newNode.Position, neighbor.Position) + newNode.Cost;
            if (newCost < neighbor.Cost && IsCollisionFree(neighbor.Position, newNode.Position, null, 0f))
            {
                neighbor.Parent = newNode;
                neighbor.Cost = newCost;
            }
        }

        return neighbors;
    }
}
