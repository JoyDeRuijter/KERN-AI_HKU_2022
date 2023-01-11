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
    private Node currentNode;
    private Dictionary<Vector2Int, Node> gridNodes = new Dictionary<Vector2Int, Node>();

    public List<Vector2Int> FindPathToTarget(Vector2Int startPos, Vector2Int endPos, Cell[,] grid)
    {
        List<Vector2Int> path = new List<Vector2Int>();
        
        openNodes.Clear();
        closedNodes.Clear();
        gridNodes.Clear();

        foreach (Cell cell in grid)
        {
            Node newNode = CreateNewNode(startPos, endPos, cell.gridPosition, null);
            gridNodes.Add(cell.gridPosition, newNode);
        }

        Node startNode = gridNodes[startPos];
        openNodes.Add(startNode);

        while (openNodes.Count != 0)
        {
            currentNode = GetLowestFScoreNode();
            openNodes.Remove(currentNode);
            closedNodes.Remove(currentNode);

            if (currentNode.position == endPos)
                break;

            openNodes.Remove(currentNode);
            closedNodes.Add(currentNode);

            List<Node> neighbourNodes = GetNeighbourNodes(NodeToCell(currentNode, grid), currentNode.position, endPos, currentNode, grid);

            foreach (Node neighbour in neighbourNodes)
            {
                if (closedNodes.Contains(neighbour))
                    continue;

                if (HasWall(currentNode, neighbour, grid))
                    continue;

                if (neighbour.FScore <= currentNode.FScore || !openNodes.Contains(neighbour))
                {
                    neighbour.GScore = currentNode.GScore++;
                    neighbour.HScore = currentNode.HScore;
                    neighbour.parent = currentNode;

                    if (!openNodes.Contains(neighbour))
                        openNodes.Add(neighbour);
                }
            }
        }

        while (currentNode.parent != null)
        {
            path.Add(currentNode.position);
            currentNode = currentNode.parent;
        }

        path.Reverse();
        return path;
    }

    private float CalculateScore(Vector2Int pos, Vector2Int targetPos)
    {
        return (int)Mathf.Abs(pos.x - targetPos.x) + Mathf.Abs(pos.y - targetPos.y);
    }

    private Cell NodeToCell(Node node, Cell[,] grid)
    {
        Cell c = grid[node.position.x, node.position.y];
        if (c == null)
        { 
            Debug.Log($"NODE WITH POSITION: {node.position} COULD NOT BE CAST TO A CELL");
            return null;
        }
        else
            return c;
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
        newNode.parent = null;

        if (parent == null)
            newNode.GScore = CalculateScore(startPos, position);
        else
            newNode.GScore = parent.GScore + CalculateScore(startPos, position);

        newNode.HScore = CalculateScore(endPos, position);
        return newNode;
    }

    private List<Node> GetNeighbourNodes(Cell cell, Vector2Int startPos, Vector2Int endPos, Node parent, Cell[,] grid)
    {
        if (cell == null)
        {
            Debug.Log("CAN'T GET NEIGHBOURNODES SINCE 'cell' IS NULL");
            return null;
        }

        List<Node> neighbours = new List<Node>();
        foreach (Cell c in cell.GetNeighbours(grid))
            neighbours.Add(gridNodes[c.gridPosition]);

        return neighbours;
    }

    private bool HasWall(Node currentNode, Node neighbourNode, Cell[,] grid)
    { 
        Vector2Int neighbourDir = currentNode.position - neighbourNode.position;
        Cell neighbourCell = grid[neighbourNode.position.x, neighbourNode.position.y];

        if (neighbourDir == new Vector2Int(1, 0) && neighbourCell.HasWall(Wall.RIGHT))
            return true;
        else if (neighbourDir == new Vector2Int(-1, 0) && neighbourCell.HasWall(Wall.LEFT))
            return true;
        else if (neighbourDir == new Vector2Int(0, 1) && neighbourCell.HasWall(Wall.UP))
            return true;
        else if (neighbourDir == new Vector2Int(0, -1) && neighbourCell.HasWall(Wall.DOWN))
            return true;
       
        return false;
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
