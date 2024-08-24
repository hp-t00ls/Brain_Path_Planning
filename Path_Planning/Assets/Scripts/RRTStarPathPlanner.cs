using UnityEngine;
using System.Collections.Generic;

public class RRTStarPathPlanner : MonoBehaviour
{
    public Transform startPosition;
    public Transform targetPosition;
    public float stepSize = 1.0f;
    public float goalBias = 0.05f;
    public int maxIterations = 100000;
    public float safeDistance = 1.0f;
    public float rewireRadius = 5.0f; // Rayon pour réajuster les voisins

    private List<Node> nodes = new List<Node>();
    private float pathLength;

    void Start()
    {
        RunRRTStar();
    }

    void RunRRTStar()
    {
        nodes.Add(new Node(startPosition.position));

        for (int i = 0; i < maxIterations; i++)
        {
            Vector3 randomPoint = Utilities.GetRandomPoint(goalBias, targetPosition);
            Node nearestNode = Utilities.GetNearestNode(nodes, randomPoint);
            Vector3 newPoint = Utilities.Steer(nearestNode.Position, randomPoint, stepSize);

            if (Utilities.IsCollisionFree(nearestNode.Position, newPoint, targetPosition, safeDistance))
            {
                float newCost = Vector3.Distance(nearestNode.Position, newPoint) + nearestNode.Cost;
                Node newNode = new Node(newPoint, nearestNode, newCost);
                nodes.Add(newNode);

                // Réajuster les voisins
                Utilities.RewireNodes(nodes, newNode, rewireRadius);

                if (Vector3.Distance(newNode.Position, targetPosition.position) < stepSize)
                {
                    Debug.Log("Target reached!");

                    List<Vector3> path = Utilities.ExtractPath(newNode);

                    // Lissage du chemin avec Catmull-Rom
                    List<Vector3> smoothedPath = Utilities.SmoothPath(path, 20); // Plus de points d'interpolation pour un lissage plus précis

                    // Calcul de la longueur du chemin lissé
                    pathLength = Utilities.CalculatePathLength(smoothedPath);

                    // Visualiser le chemin lissé
                    for (int j = 0; j < smoothedPath.Count - 1; j++)
                    {
                        Debug.DrawLine(smoothedPath[j], smoothedPath[j + 1], Color.green, 20f);
                    }

                    Debug.Log("Smoothed Path Length: " + pathLength);
                    return;
                }
            }
        }
        Debug.Log("Max iterations reached without finding target.");
    }
}