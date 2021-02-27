using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class implements the A* algorithm.
/// width and height set the number of columns and rows for the grid the algorithm is working on
/// </summary>
public class AstarPathfinder
{
    private Grid<GridCell> grid;
    private List<GridCell> openList;
    private List<GridCell> closedList;

    public AstarPathfinder(int width, int heigth)
    {
        float cellSize = 1f;
        Vector3 offset = new Vector3(-17f, 0f, -11f);// Vector3.zero;
        grid = new Grid<GridCell>(width, heigth, cellSize, offset, (Grid<GridCell> g, int x, int y) => new GridCell(g, x, y));
    }

    /// <summary>
    /// This function returns a list of cells, which create the optimal path regarding distance and the position of the raysource.
    /// </summary>
    public List<GridCell> find_path(int start_x, int start_y, int end_x, int end_y)
    {
        //Transform the coordinates to the cells
        GridCell end_cell = grid.getValue(end_x, end_y);
        //Add the start Cell to the open list and create the open list
        GridCell start_cell = grid.getValue(start_x, start_y);
        openList = new List<GridCell> {start_cell};

        //Create the (empty) closed List
        closedList = new List<GridCell>();

        //initialize the grid-cells for A*-Algorithm. Start with high gCosts
        for (int x = 0; x < grid.getWidth(); x++)
        {
            for (int y = 0; y < grid.getHeight(); y++)
            {
                GridCell gridCell = grid.getValue(x, y);
                gridCell.set_gCost(100000f);                             //initialize with a very high value. This is necessary, because it must be a higher value than the real value which is used later in the algorithm
                gridCell.determine_fCost();
                gridCell.set_previousCell(null);
                gridCell.set_hCost((int)(Mathf.Sqrt(Mathf.Pow(end_x - gridCell.get_x(), 2) + Mathf.Pow(end_y - gridCell.get_y(), 2)) * grid.getCellSize()));
            }
        }

        start_cell.set_gCost(0);
        start_cell.determine_fCost();
        
        while (openList.Count > 0)
        {
            GridCell currentCell = get_next_cell(openList);
            //test if the destination is reached
            if(currentCell == end_cell)
            {
                return construct_path(currentCell);                                                      //return the path, use the function to reconstruct it from the single cells und wrap it all up
            }

            //if the current cell is not the end cell, add neighbours to the open list
            List<GridCell> neighbour_cells = get_neighbour_cells(currentCell);
            for(int i = 0; i < neighbour_cells.Count; i++)
            {
                //Filter out blocked an already walked cells
                if (neighbour_cells[i].get_blocked())
                {
                    closedList.Add(neighbour_cells[i]);
                }
                if (closedList.Contains(neighbour_cells[i]))
                {
                    continue;
                }


                float new_gCost = determine_hCost(currentCell, neighbour_cells[i]) + currentCell.get_gCost();           //hCost function can be used for gCost i this point, because the cells are neighbours and the direct way from cell to cell is possible in each case
                new_gCost += neighbour_cells[i].get_additionalCost();


                //if the path to the neighbor is better than existing paths, set the new path and adapt the neighbour values
                if(new_gCost < neighbour_cells[i].get_gCost())
                {
                    neighbour_cells[i].set_previousCell(currentCell);
                    neighbour_cells[i].set_gCost(new_gCost);
                    neighbour_cells[i].determine_fCost();

                    if(!(openList.Contains(neighbour_cells[i]))) openList.Add(neighbour_cells[i]);                       //Add the new cell to the open List
                }
            }
            closedList.Add(currentCell);
            openList.Remove(currentCell);
        }
        //In this point, the openList is empty and no path could be found. This means there is no path from start to end
        return null;
    }


    /// <summary>
    /// Returns the next grid-cell, the cell from the open list with the lowest (approximated) fCost 
    /// </summary>
    private GridCell get_next_cell(List<GridCell> openList)
    {
        GridCell bestChoice = openList[0];
        for(int i = 0; i < openList.Count; i++)
        {
            if (openList[i].get_fCost() < bestChoice.get_fCost())
            {
                bestChoice = openList[i];
            }
        }
        return bestChoice;
    }


    /// <summary>
    /// Returns the adjacent cells to a given cell
    /// </summary>
    public List<GridCell> get_neighbour_cells(GridCell midCell)
    {
        List<GridCell> neighbour_cells = new List<GridCell>();
        int x = midCell.get_x();
        int y = midCell.get_y();

        for (int x_offset = -1; x_offset < 2; x_offset++)
        {
            for (int y_offset = -1; y_offset < 2; y_offset++)
            {
                if (!(x_offset == 0 && y_offset == 0))
                {
                    GridCell tempCell = grid.getValue(x + x_offset, y + y_offset);
                    if (!(tempCell is null)) {
                        neighbour_cells.Add(tempCell);
                    }
                }
            }
        }

        return neighbour_cells;
    }

    /// <summary>
    /// Returns the h-costs as the direct distance between two cells
    /// </summary>
    private int determine_hCost(int start_x, int start_y, int end_x, int end_y)
    {
        return (int)(Mathf.Sqrt(Mathf.Pow(end_x - start_x, 2) + Mathf.Pow(end_y - start_y, 2)) * grid.getCellSize());
    }
    private int determine_hCost(GridCell start_cell, GridCell end_cell)
    {
        int start_x = start_cell.get_x();
        int start_y = start_cell.get_y();
        int end_x = end_cell.get_x();
        int end_y = end_cell.get_y();
        return determine_hCost(start_x, start_y, end_x, end_y);
    }

    /// <summary>
    /// Constructs a path from the end cell, using the previous cell of every cell in the path 
    /// </summary>
    private List<GridCell> construct_path(GridCell end_cell)
    {
        List<GridCell> path = new List<GridCell>();
        path.Add(end_cell);
        GridCell current_cell = end_cell;

        while(current_cell.get_previousCell() != null)              //the while stops at the startNode
        {
            current_cell = current_cell.get_previousCell();
            path.Add(current_cell);
        }
        path.Reverse();
        return path;
    }

    /// <summary>
    /// Creates a (theoretical) ray source in the given cell. The ray source is only used for calculations
    /// </summary>
    public void create_raySource(int x, int z)
    {
        GridCell new_rS = grid.getValue(x, z);
        if (!(new_rS.get_blocked())) {
            grid.getValue(x, z).set_blocked();
        }
        grid.setRaySource(x, z);
        float cellSize = this.grid.getCellSize();
        Debug.DrawLine(new Vector3(x, 0, z) * cellSize,
                       new Vector3(x + 1, 0, z + 1) * cellSize,
                       Color.cyan,
                       400f);
        Debug.DrawLine(new Vector3(x, 0, z + 1) * cellSize,
                       new Vector3(x + 1, 0, z) * cellSize,
                       Color.cyan,
                       400f);
        determine_additionalCost();
    }

    /// <summary>
    /// Calculates the costs created by the ray source. In the code are different methods for this calculation, which can be used
    /// </summary>
    public void determine_additionalCost()
    {
        float weight = 500f;
        if (grid.getRaySource() != null)
        {
            for (int x = 0; x < grid.getWidth(); x++)
            {
                for (int y = 0; y < grid.getHeight(); y++)
                {
                    if (!(grid.getRaySource().get_x() == x && grid.getRaySource().get_y() == y)) { 
                        //Direct distance
                        grid.getValue(x, y).set_additionalCost(1f / (Mathf.Sqrt(Mathf.Pow(grid.getRaySource().get_x() - x, 2) + Mathf.Pow(grid.getRaySource().get_y() - y, 2)) * grid.getCellSize()) * weight);


                        //Manhattan distance:
                        //Debug.Log(x  + "," + y + ":  " + (1 / (Mathf.Abs((grid.getRaySource().get_x() - x)) + Mathf.Abs(grid.getRaySource().get_y() - y)) * weight));
                        //grid.getValue(x, y).set_additionalCost(1f / (Mathf.Abs((grid.getRaySource().get_x() - x)) + Mathf.Abs(grid.getRaySource().get_y() - y)) * weight);


                        //Calculate RayDose:
                        /*float WEIGHT = 75f;
                        float distance = (Mathf.Sqrt(Mathf.Pow(grid.getRaySource().get_x() - x, 2) + Mathf.Pow(grid.getRaySource().get_y() - y, 2)) * grid.getCellSize());
                        float watt = 50;
                        float time = 1;
                        if (Double.IsNaN(distance)) {
                            grid.getValue(x, y).set_additionalCost(0f); 
                        } else { 
                            distance = Mathf.Abs(distance);
                            float sourceEnergy = watt * time;
                            float energyDose = sourceEnergy / WEIGHT;
                            energyDose = energyDose / (distance * distance);
                            grid.getValue(x, y).set_additionalCost((energyDose)*weight);
                        }*/
                    }
                }
            }
        }
    }


    public Grid<GridCell> GetGrid()
    {
        return grid;
    }
    
}
