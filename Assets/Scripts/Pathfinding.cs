using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    private GridManager grid;

    void Awake()
    {
        grid = GetComponent<GridManager>();
    }

    public List<Node> FindPath(Vector2 startPos, Vector2 targetPos)
    {
        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(targetPos);

        if (startNode == null || targetNode == null)
            return null;

        if (!startNode.walkable || !targetNode.walkable)
            return null;

        Node[,] allNodes = grid.GetGrid();

        for (int x = 0; x < allNodes.GetLength(0); x++)
        {
            for (int y = 0; y < allNodes.GetLength(1); y++)
            {
                allNodes[x, y].gCost = int.MaxValue;
                allNodes[x, y].hCost = 0;
                allNodes[x, y].parent = null;
            }
        }

        startNode.gCost = 0;
        startNode.hCost = GetDistance(startNode, targetNode);

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();

        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0];

            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < currentNode.fCost ||
                    (openSet[i].fCost == currentNode.fCost &&
                     openSet[i].hCost < currentNode.hCost))
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == targetNode)
            {
                return RetracePath(startNode, targetNode);
            }

            foreach (Node neighbor in grid.GetNeighbors(currentNode))
            {
                if (neighbor == null)
                    continue;

                if (!neighbor.walkable || closedSet.Contains(neighbor))
                    continue;

                int newCost = currentNode.gCost + GetDistance(currentNode, neighbor);

                if (newCost < neighbor.gCost || !openSet.Contains(neighbor))
                {
                    neighbor.gCost = newCost;
                    neighbor.hCost = GetDistance(neighbor, targetNode);
                    neighbor.parent = currentNode;

                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Add(neighbor);
                    }
                }
            }
        }

        return null;
    }

    List<Node> RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();

        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);

            if (currentNode.parent == null)
                return null;

            currentNode = currentNode.parent;
        }

        path.Reverse();

        return path;
    }

    int GetDistance(Node a, Node b)
    {
        int dstX = Mathf.Abs(a.gridX - b.gridX);
        int dstY = Mathf.Abs(a.gridY - b.gridY);

        return dstX + dstY;
    }
}