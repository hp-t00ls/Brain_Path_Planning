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

    public static void ExtractPath(Node endNode)
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
    }
}
