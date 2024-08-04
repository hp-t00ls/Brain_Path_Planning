using UnityEngine;

public class Node
{
    public Vector3 Position;
    public Node Parent;

    public Node(Vector3 position, Node parent = null)
    {
        Position = position;
        Parent = parent;
    }
}

