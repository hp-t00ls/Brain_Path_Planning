using UnityEngine;

public class Node
{
    public Vector3 Position;
    public Node Parent;
    public float Cost; // Coût du chemin du nœud racine à ce nœud

    // Constructeur pour un nœud avec coût
    public Node(Vector3 position, Node parent = null, float cost = 0f)
    {
        Position = position;
        Parent = parent;
        Cost = cost;
    }
}