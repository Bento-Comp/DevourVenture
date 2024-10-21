using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid<TGridObject>
{
    private float m_nodeSize;
    private int m_width;
    private int m_height;
    private TGridObject[,] m_gridArray;


    public Grid(int width, int height, float nodeSize, Func<Grid<TGridObject>, int, int, TGridObject> createGridObject)
    {
        this.m_height = height;
        this.m_width = width;
        this.m_nodeSize = nodeSize;

        m_gridArray = new TGridObject[m_width, m_height];


        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                m_gridArray[i, j] = createGridObject(this, i, j);
            }
        }
    }


    public TGridObject GetGridObject(int x, int y)
    {
        return m_gridArray[x, y];
    }

    public int GetWidth()
    {
        return m_width;
    }

    public int GetHeight()
    {
        return m_height;
    }

    public void DisplayGridContent()
    {
        for (int i = 0; i < m_width; i++)
        {
            for (int j = 0; j < m_height; j++)
            {
                Debug.Log(m_gridArray[i, j].ToString());
            }
        }
    }


}
