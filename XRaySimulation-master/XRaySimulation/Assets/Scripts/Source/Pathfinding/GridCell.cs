using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class contains all information needed for an A* algorithm. An object of this class is used for every cell of the grid,
/// which is used for the algorithm.
/// </summary>
public class GridCell
{
    private int hCost;      //approximated costs to the end
    private float gCost;      //costs from start to current cell/node
    private float fCost;      //hCost + gCost
    private float additionalCost;

    private GridCell previousCell;


    private Grid<GridCell> grid;
    private int x;
    private int y;

    private bool is_blocked;
    public GridCell(Grid<GridCell> grid, int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
        this.is_blocked = false;
        this.additionalCost = 0f;
    }

    /// <summary>
    /// Determines the f-costs (total costs) for an object of the class. The costs are
    /// f-costs = g-costs + h-costs + additionalCost
    /// </summary>
    public void determine_fCost()
    {
        fCost = gCost + hCost + additionalCost;
    }

    /// <summary>
    /// Shows a blocked cell as blocked in the scene
    /// </summary>
    public void draw_blocked(float duration)
    {
        if (this.is_blocked)
        {
            float cellSize = this.grid.getCellSize();
            Debug.DrawLine(new Vector3(this.x * cellSize, 0, this.y * cellSize) + grid.getOffset(), new Vector3((this.x + 1) * cellSize, 0, (this.y + 1) * cellSize) + grid.getOffset(), Color.blue, duration);
        }
        
    }

    //setter
    public void set_hCost(int hCost)
    {
        this.hCost = hCost;
    }
    public void set_gCost(float gCost)
    {
        this.gCost = gCost;
    }
    public void set_fCost(int fCost)
    {
        this.fCost = fCost;
    }
    public void  set_previousCell(GridCell previousCell)
    {
        this.previousCell = previousCell;
    }
    /// <summary>
    /// This function marks a cell as blocked or unblocked, depending on the current state. No path can pass over a blocked cell.
    /// </summary>
    public void set_blocked()
    {
        if(is_blocked)
        {
            is_blocked = false;
        } else
        {
            is_blocked = true;
        }
    }

    public void set_additionalCost(float aC)
    {
        this.additionalCost = aC;
    }

    //getter
    public int get_hCost()
    {
        return hCost;
    }
    public float get_gCost()
    {
        return gCost;
    }
    public float get_fCost()
    {
        return fCost;
    }
    public GridCell get_previousCell()
    {
        return previousCell;
    }
    public int get_x()
    {
        return this.x;
    }
    public int get_y()
    {
        return this.y;
    }
    public Grid<GridCell> get_grid()
    {
        return this.grid;
    }
    public bool get_blocked()
    {
        return is_blocked;
    }
    public float get_additionalCost()
    {
        return this.additionalCost;
    }
}

