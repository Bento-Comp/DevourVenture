using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GridPathFinding
{
    private const int STRAIGHT_MOVE_COST = 10;
    private const int DIAGONAL_MOVE_COST = 11;


    private List<Node> m_openList;
    private List<Node> m_closedList;
    private Grid<Node> m_grid;


    public Grid<Node> Grid { get => m_grid; }



    public GridPathFinding(int width, int height, float nodeSize)
    {
        m_grid = new Grid<Node>(width, height, nodeSize, (Grid<Node> grid, int x, int y) => new Node(grid, x, y));
    }


    public List<Node> FindPath(int startX, int startY, int endX, int endY, bool canMoveDiagonaly)
    {
        Node startNode = m_grid.GetGridObject(startX, startY);
        Node endNode = m_grid.GetGridObject(endX, endY);

        m_openList = new List<Node>() { startNode };
        m_closedList = new List<Node>();


        for (int i = 0; i < m_grid.GetWidth(); i++)
        {
            for (int j = 0; j < m_grid.GetHeight(); j++)
            {
                Node node = m_grid.GetGridObject(i, j);
                node.gCost = int.MaxValue;
                node.CalculateFCost();
                node.cameFromNode = null;
            }
        }

        startNode.gCost = 0;
        startNode.hCost = CalculateDistanceCost(startNode, endNode);
        startNode.CalculateFCost();

        while (m_openList.Count > 0)
        {
            Node currentNode = GetLowestFCostNode(m_openList);

            if (currentNode == endNode)
            {
                return CalculatePath(endNode);
            }


            m_openList.Remove(currentNode);
            m_closedList.Add(currentNode);


            foreach (Node neighbourNode in GetNeighboursList(currentNode, canMoveDiagonaly))
            {
                if (m_closedList.Contains(neighbourNode))
                    continue;

                if (!neighbourNode.m_isWalkable)
                {
                    m_closedList.Add(neighbourNode);
                    continue;
                }

                int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNode, neighbourNode);

                if (tentativeGCost < neighbourNode.gCost)
                {
                    neighbourNode.cameFromNode = currentNode;
                    neighbourNode.gCost = tentativeGCost;
                    neighbourNode.hCost = CalculateDistanceCost(neighbourNode, endNode);
                    neighbourNode.CalculateFCost();
                }

                if (!m_openList.Contains(neighbourNode))
                {
                    m_openList.Add(neighbourNode);
                }
            }
        }

        return null;
    }


    private List<Node> GetNeighboursList(Node currentNode, bool checkDiagonalNeighbours)
    {
        List<Node> neighboursList = new List<Node>();

        //check up
        if (currentNode.Y + 1 < m_grid.GetHeight())
            neighboursList.Add(GetNode(currentNode.X, currentNode.Y + 1));

        //check down
        if (currentNode.Y - 1 >= 0)
            neighboursList.Add(GetNode(currentNode.X, currentNode.Y - 1));

        //check left
        if (currentNode.X - 1 >= 0)
        {
            neighboursList.Add(GetNode(currentNode.X - 1, currentNode.Y));

            if (checkDiagonalNeighbours)
            {
                if (currentNode.Y - 1 >= 0)
                    neighboursList.Add(GetNode(currentNode.X - 1, currentNode.Y - 1));

                if (currentNode.Y + 1 < m_grid.GetHeight())
                    neighboursList.Add(GetNode(currentNode.X - 1, currentNode.Y + 1));
            }
        }

        //check right
        if (currentNode.X + 1 < m_grid.GetWidth())
        {
            neighboursList.Add(GetNode(currentNode.X + 1, currentNode.Y));

            if (checkDiagonalNeighbours)
            {
                if (currentNode.Y - 1 >= 0)
                    neighboursList.Add(GetNode(currentNode.X + 1, currentNode.Y - 1));

                if (currentNode.Y + 1 < m_grid.GetHeight())
                    neighboursList.Add(GetNode(currentNode.X + 1, currentNode.Y + 1));
            }
        }



        return neighboursList;
    }


    public Node GetNode(int x, int y)
    {
        return m_grid.GetGridObject(x, y);
    }


    private List<Node> CalculatePath(Node endNode)
    {
        List<Node> pathNodeList = new List<Node>();

        pathNodeList.Add(endNode);

        Node currentNode = endNode;

        while (currentNode.cameFromNode != null)
        {
            pathNodeList.Add(currentNode.cameFromNode);
            currentNode = currentNode.cameFromNode;
        }

        pathNodeList.Reverse();

        return pathNodeList;
    }


    private Node GetLowestFCostNode(List<Node> nodeList)
    {
        Node lowestFCostNode = nodeList[0];

        for (int i = 1; i < nodeList.Count; i++)
        {
            if (nodeList[i].fCost < lowestFCostNode.fCost)
                lowestFCostNode = nodeList[i];
        }

        return lowestFCostNode;
    }


    private int CalculateDistanceCost(Node a, Node b)
    {
        int xDistance = Mathf.Abs(a.X - b.X);
        int yDistance = Mathf.Abs(a.Y - b.Y);
        int remaining = Mathf.Abs(xDistance - yDistance);

        return DIAGONAL_MOVE_COST * Mathf.Min(xDistance, yDistance) + STRAIGHT_MOVE_COST * remaining;
    }
}
