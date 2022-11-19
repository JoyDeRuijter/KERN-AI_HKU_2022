using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Astar
{
    /// <summary>
    /// TODO: Implement this function so that it returns a list of Vector2Int positions which describes a path
    /// Note that you will probably need to add some helper functions
    /// from the startPos to the endPos
    /// </summary>
    /// <param name="startPos"></param>
    /// <param name="endPos"></param>
    /// <param name="grid"></param>
    /// <returns></returns>

    private List<Node> openNodes = new List<Node>();
    private List<Node> closedNodes = new List<Node>();
    private List<Node> neighbourNodes = new List<Node>();
    private Node currentNode;

    public List<Vector2Int> FindPathToTarget(Vector2Int startPos, Vector2Int endPos, Cell[,] grid)
    {
        List<Vector2Int> path = new List<Vector2Int>();
        Node startNode = CreateNewNode(startPos, endPos, startPos, null);
        openNodes.Add(startNode);

        while (openNodes.Count != 0)
        {
            currentNode = GetLowestFScoreNode();

            if (currentNode.position == endPos)
                return Path(currentNode);

            openNodes.Remove(currentNode);
            closedNodes.Add(currentNode);

            neighbourNodes = GetNeighbourNodes(NodeToCell(currentNode, grid), startPos, endPos, currentNode, grid);
            foreach (Node n in neighbourNodes)
            { 
                if(closedNodes.Contains(n)) // Add || if it has a wall between n and it's parent
                    continue;

                if (!openNodes.Contains(n))
                    openNodes.Add(n);
                else 
                {
                    // ??? IDK HOW TO DO THIS PART
                    // actualNode = openNodes.find(node)
                    // If node.GScore < actualNode.Gscore
                        // actualNode.GScore = node.GScore
                        // actualNode.HScore = node.HScore
                        // actualNode.parent = node.parent
                }
            } 
        }
        Debug.Log("UNABLE TO REACH END POSITION");
        return null;
    }

    private float CalculateGScore(Node node, Node parent)
    {
        return parent.GScore + Vector2Int.Distance(node.position, parent.position);
    }

    private float CalculateHScore(Node node, Vector2Int startPos, Vector2Int endPos)
    {
        // ??? IDK HOW TO DO THIS PART
        return 0f;
    }

    private Cell NodeToCell(Node node, Cell[,] grid)
    {
        foreach (Cell c in grid)
        { 
            if(c.gridPosition == node.position)
                return c;
        }
        Debug.Log($"NODE WITH POSITION: {node.position} COULD NOT BE CAST TO A CELL");
        return null;
    }

    private Node GetLowestFScoreNode()
    {
        Node currentNode;
        Node lastNode = null;
        Node finalNode = null;

        foreach (Node n in openNodes)
        { 
            currentNode = n;

            if(lastNode == null || currentNode.FScore < lastNode.FScore)
                finalNode = currentNode;

            lastNode = currentNode;
        }
        return finalNode;
    }

    private Node CreateNewNode(Vector2Int startPos, Vector2Int endPos, Vector2Int position, Node parent)
    {
        Node newNode = new Node();
        newNode.position = position;

        if (parent != null)
        {
            newNode.parent = parent;
            newNode.GScore = CalculateGScore(newNode, parent);
        }
        else
        {
            newNode.parent = null;
            newNode.GScore = 0f;
        }

        newNode.HScore = CalculateHScore(newNode, startPos, endPos);
        return newNode;
    }

    private List<Node> GetNeighbourNodes(Cell cell, Vector2Int startPos, Vector2Int endPos, Node parent, Cell[,] grid)
    {
        List<Node> neighbours = new List<Node>();
        foreach (Cell c in cell.GetNeighbours(grid))
        {
            Node n = CreateNewNode(startPos, endPos, c.gridPosition, parent);
            neighbours.Add(n);
        }
        return neighbours;
    }

    private List<Vector2Int> Path(Node node)
    {
        // TODO Write this function
        return new List<Vector2Int>();
    }

    /// <summary>
    /// This is the Node class you can use this class to store calculated FScores for the cells of the grid, you can leave this as it is
    /// </summary>
    public class Node
    {
        public Vector2Int position; //Position on the grid
        public Node parent; //Parent Node of this node

        public float FScore { //GScore + HScore
            get { return GScore + HScore; }
        }
        public float GScore; //Current Travelled Distance
        public float HScore; //Distance estimated based on Heuristic

        public Node() { }
        public Node(Vector2Int position, Node parent, int GScore, int HScore)
        {
            this.position = position;
            this.parent = parent;
            this.GScore = GScore;
            this.HScore = HScore;
        }
    }
}
