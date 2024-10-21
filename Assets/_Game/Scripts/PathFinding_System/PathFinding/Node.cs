using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    private Grid<Node> m_grid;
    private int m_x;
    private int m_y;

    public int gCost;
    public int hCost;
    public int fCost;

    public bool m_isWalkable;

    public Node cameFromNode;

    public int X { get => m_x; }
    public int Y { get => m_y; }


    public Node(Grid<Node> grid, int x, int y)
    {
        m_x = x;
        m_y = y;
        m_grid = grid;
        m_isWalkable = true;
    }

    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }

    public override string ToString()
    {
        return "[" + m_x + ", " + m_y + "] - " + "g : " + gCost + " / f : " + fCost + " / h : " + hCost;
    }
}
