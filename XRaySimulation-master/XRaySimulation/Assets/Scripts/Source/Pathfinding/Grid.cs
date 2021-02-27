using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class simulates and draws a grid of objects. Width and height determine the number of rows and rows of the grid. 
/// Offset determines the position in space. The function required in the constructor creates the individual objects from which the grid is built.
/// </summary>
public class Grid<GridObject>       //use a generic so A* is working with "GridCell" class
{
    private int width;
    private int height;
    private float cellSize;
    private Vector3 offset;
    private GridObject[,] gridValues; //2-dimensional Array
    private GridObject raySource;

public Grid(int width, int height, float cellSize, Vector3 offset, Func<Grid<GridObject>, int, int, GridObject> new_grid_object)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.offset = offset;
        this.gridValues = new GridObject[width, height];


        //Fill the grid with gridObjects
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                gridValues[x, y] = new_grid_object(this, x, y);
            }
        }

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Debug.DrawLine(getRealPosition(x, y), getRealPosition(x, y) + (new Vector3(1, 0f, 0f) * cellSize), Color.white, 500f);
                Debug.DrawLine(getRealPosition(x, y), getRealPosition(x, y) + (new Vector3(0f, 0f, 1) * cellSize), Color.white, 500f);
            }
        }
        Debug.DrawLine(getRealPosition(0, height), getRealPosition(width, height), Color.white, 500f);
        Debug.DrawLine(getRealPosition(width, 0), getRealPosition(width, height), Color.white, 500f);
    }

    /// <summary>
    /// Returns the relative position to the parent object of cell (x, y)
    /// </summary>
    private Vector3 getRealPosition(int x, int y)
    {
        return new Vector3(x, 0f, y) * cellSize + offset;
    }

    /// <summary>
    /// Returns the row and column of the cell, in which a given point is located
    /// </summary>
    public void findCell(Vector3 worldPosition, out int x, out int y)
    {
        if(worldPosition.x - offset.x > 0 && worldPosition.z - offset.z > 0)
        {
            x = (int)((worldPosition.x - offset.x) / cellSize);
            y = (int)((worldPosition.z - offset.z) / cellSize);
        } 
        else {
            x = 0;
            y = 0;
        }
        
    }

    public void setValue(int x, int y, GridObject value)
    {
        // test here if a value is valid, for example only non negative numbers in A*
        if(x >= 0 && x <= width && y >= 0 && y <= height)
        {
            gridValues[x, y] = value;
        }  
    }
    public void setValue(Vector3 worldPosition, GridObject value)
    {
        int x, y;
        findCell(worldPosition, out x, out y);
        setValue(x, y, value);
    }
    public void setRaySource(GridObject rS)
    {
        this.raySource = rS;
    }
    public void setRaySource(int x, int y)
    {
        this.raySource = gridValues[x, y];
    }

    public GridObject getValue(int x, int y)
    {
        if (x >= 0 && x < width && y >= 0 && y < height)
        {
            return gridValues[x, y];
        } else
        {
            return default;
        }
    }
    public GridObject getValue(Vector3 worldPosition)
    {
        int x, y;
        findCell(worldPosition, out x, out y);
        return getValue(x, y);
    }

    public int getWidth()
    {
        return width;
    }
    public int getHeight()
    {
        return height;
    }
    public float getCellSize()
    {
        return cellSize;
    }
    public GridObject getRaySource()
    {
        return this.raySource;
    }
    public Vector3 getOffset()
    {
        return this.offset;
    }
}
