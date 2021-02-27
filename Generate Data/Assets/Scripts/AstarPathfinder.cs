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
        float cellSize = 10f;
        Vector3 offset = Vector3.zero;
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
                return construct_path(currentCell);                     //return the path, use the function to reconstruct it from the single cells und wrap it all up
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


                float new_gCost = determine_new_gCost(currentCell, neighbour_cells[i]) + currentCell.get_gCost();           //hCost function can be used for gCost i this point, because the cells are neighbours and the direct way from cell to cell is possible in each case
                new_gCost += neighbour_cells[i].get_additionalCost();


                //if the path to the neighbor is better than existing paths, set the new path and adapt the neighbour values
                if(new_gCost < neighbour_cells[i].get_gCost())
                {
                    neighbour_cells[i].set_previousCell(currentCell);
                    neighbour_cells[i].set_gCost(new_gCost);
                    neighbour_cells[i].determine_fCost();

                    if(!(openList.Contains(neighbour_cells[i]))) openList.Add(neighbour_cells[i]);                           //Add the new cell to the open List
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
    /// Returns the new g-costs as the direct distance between two cells
    /// </summary>
    private int determine_new_gCost(int start_x, int start_y, int end_x, int end_y)
    {
        return (int)(Mathf.Sqrt(Mathf.Pow(end_x - start_x, 2) + Mathf.Pow(end_y - start_y, 2)) * grid.getCellSize());
    }
    private int determine_new_gCost(GridCell start_cell, GridCell end_cell)
    {
        int start_x = start_cell.get_x();
        int start_y = start_cell.get_y();
        int end_x = end_cell.get_x();
        int end_y = end_cell.get_y();
        return determine_new_gCost(start_x, start_y, end_x, end_y);
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
    public void create_raySource(int x, int y)
    {
        GridCell new_rS = grid.getValue(x, y);
        if (!(new_rS.get_blocked())) {
            grid.getValue(x, y).set_blocked();
        }
        grid.setRaySource(x, y);
        float cellSize = this.grid.getCellSize();
        Debug.DrawLine(new Vector3(x, y) * cellSize,
                       new Vector3(x + 1, y + 1) * cellSize,
                       Color.cyan,
                       400f);
        Debug.DrawLine(new Vector3(x, y + 1) * cellSize,
                       new Vector3(x + 1, y) * cellSize,
                       Color.cyan,
                       400f);
        determine_additionalCost();
    }

    /// <summary>
    /// Calculates the costs created by the ray source. In the code are different methods for this calculation, which can be used
    /// </summary>
    public void determine_additionalCost()
    {
        float weight = 500f;         //5000f
        if (grid.getRaySource() != null)
        {
            for (int x = 0; x < grid.getWidth(); x++)
            {
                for (int y = 0; y < grid.getHeight(); y++)
                {
                    if (!(grid.getRaySource().get_x() == x && grid.getRaySource().get_y() == y)) { 
                        //Direct distance:
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

                        //TODO: The Formula for additional Costs must be improved (not only 1/... * weight). Test if it is better to add the add.Costs to the hCosts (effects in get_next_cell)
                        //Basic
                        //grid.getValue(x, y).set_additionalCost();
                    }
                }
            }
        }
    }

    /// <summary>
    /// Creates a (theoretical) cabinet on the given cell position. The cabinet needs 3x1 cells.
    /// The cabinet is always at the border of the grid.
    /// </summary>
    public void create_cabinet(int x_position, int y_position, int grid_side)
    {
        if (grid.getValue(x_position, y_position).get_furniture_availability())
        {
            if (grid_side == 0 || grid_side == 2)
            {
                //set blocked and "furnitured"
                grid.getValue(x_position - 1, y_position).set_blocked();
                grid.getValue(x_position + 1, y_position).set_blocked();
                grid.getValue(x_position - 1, y_position).set_furniture_availability(false);
                grid.getValue(x_position + 1, y_position).set_furniture_availability(false);

                //draw
                grid.getValue(x_position - 1, y_position).draw_furniture(150f);
                grid.getValue(x_position + 1, y_position).draw_furniture(150f);
            }
            else
            {
                //set blocked and "furnitured"
                grid.getValue(x_position, y_position - 1).set_blocked();
                grid.getValue(x_position, y_position + 1).set_blocked();
                grid.getValue(x_position, y_position - 1).set_furniture_availability(false);
                grid.getValue(x_position, y_position + 1).set_furniture_availability(false);

                //draw
                grid.getValue(x_position, y_position - 1).draw_furniture(150f);
                grid.getValue(x_position, y_position + 1).draw_furniture(150f);
            }
            grid.getValue(x_position, y_position).set_blocked();
            grid.getValue(x_position, y_position).set_furniture_availability(false);
            grid.getValue(x_position, y_position).draw_furniture(150f);

            
            List<GridCell> neighbour_cells = get_cabinet_neighbour_cells(x_position, y_position, grid_side);
            for (int i = 0; i < neighbour_cells.Count; i++)
            {
                neighbour_cells[i].set_furniture_availability(false);
            }

        } else
        {
            //You can create another cabinet here, if 2 cabinets would intersect.
        }
    }

    /// <summary>
    /// returns the 4~5 direct(!) neighbour cells of a cabinet
    /// </summary>
    public List<GridCell> get_cabinet_neighbour_cells(int x, int y, int grid_side)
    {
        List<GridCell> neighbour_cells = new List<GridCell>();

        switch (grid_side) { 
            case 0: //down
                for(int x_offset = -1; x_offset < 2; x_offset++)
                {
                    GridCell tempCell = grid.getValue(x + x_offset, y + 1);
                    if (!(tempCell is null))
                    {
                        neighbour_cells.Add(tempCell);
                    }
                }
                GridCell two_left_tempCell_down = grid.getValue(x - 2, y);
                if (!(two_left_tempCell_down is null))
                {
                    neighbour_cells.Add(two_left_tempCell_down);
                }
                GridCell two_right_tempCell_down = grid.getValue(x + 2, y);
                if (!(two_right_tempCell_down is null))
                {
                    neighbour_cells.Add(two_right_tempCell_down);
                }
                break;

            case 1: //right
                for (int y_offset = -1; y_offset < 2; y_offset++)
                {
                    GridCell tempCell = grid.getValue(x - 1, y + y_offset);
                    if (!(tempCell is null))
                    {
                        neighbour_cells.Add(tempCell);
                    }
                }
                GridCell two_down_tempCell_right = grid.getValue(x, y - 2);
                if (!(two_down_tempCell_right is null))
                {
                    neighbour_cells.Add(two_down_tempCell_right);
                }
                GridCell two_up_tempCell_right = grid.getValue(x, y + 2);
                if (!(two_up_tempCell_right is null))
                {
                    neighbour_cells.Add(two_up_tempCell_right);
                }
                break;

            case 2: //up
                for (int x_offset = -1; x_offset < 2; x_offset++)
                {
                    GridCell tempCell = grid.getValue(x + x_offset, y - 1);
                    if (!(tempCell is null))
                    {
                        neighbour_cells.Add(tempCell);
                    }
                }
                GridCell two_left_tempCell_up = grid.getValue(x - 2, y);
                if (!(two_left_tempCell_up is null))
                {
                    neighbour_cells.Add(two_left_tempCell_up);
                }
                GridCell two_right_tempCell_up = grid.getValue(x + 2, y);
                if (!(two_right_tempCell_up is null))
                {
                    neighbour_cells.Add(two_right_tempCell_up);
                }
                break;

            case 3: //left
                for (int y_offset = -1; y_offset < 2; y_offset++)
                {
                    GridCell tempCell = grid.getValue(x + 1,y + y_offset);
                    if (!(tempCell is null))
                    {
                        neighbour_cells.Add(tempCell);
                    }
                }
                GridCell two_down_tempCell_left = grid.getValue(x, y - 2);
                if (!(two_down_tempCell_left is null))
                {
                    neighbour_cells.Add(two_down_tempCell_left);
                }
                GridCell two_up_tempCell_left = grid.getValue(x, y + 2);
                if (!(two_up_tempCell_left is null))
                {
                    neighbour_cells.Add(two_up_tempCell_left);
                }
                break;

            default:
                break;
        }
        return neighbour_cells;
    }

    public List<GridCell> get_cabinet_neighbour_cells(int x, int y)
    {
        return get_cabinet_neighbour_cells(x, y, get_cabinet_side(x , y));
    }


    /// <summary>
    /// Returns the the side of the grid where a given cabinet is located
    /// </summary>
    public int get_cabinet_side(int x, int y)
    {
        if (y == 0)
        {
            return 0;
        }
        if (x == this.GetGrid().getWidth() - 1)
        {
            return 1;
        }
        if (x == 0)
        {
            return 3;
        }
        if (y == this.GetGrid().getHeight() - 1)
        {
            return 2;
        }
        //"Der Schrank steht an keiner Wand"
        return 0;
    }

    /// <summary>
    /// Returns the neighbor cell of a cabinet in a given grid-position with the smallest distance to a given startpoint
    /// </summary>
    public GridCell get_nearest_cabinet_neighbour_cell(int start_x,int start_y, int x_position, int y_position, int grid_side)
        {
            List<GridCell> neighbour_cells = get_cabinet_neighbour_cells(x_position, y_position, grid_side);
            GridCell start_position = grid.getValue(start_x, start_y);
            GridCell return_cell = neighbour_cells[0];
            int smallest_distance = determine_hCost(start_position, neighbour_cells[0]);
            if (neighbour_cells.Count != 0)
            {
                for (int i = 0; i < neighbour_cells.Count; i++)
                {
                    //Debug.Log("Nachbarzelle Nummer: " + i + " X-Wert: " + neighbour_cells[i].get_x() + ", Y-Wert: " + neighbour_cells[i].get_y());
                    if (determine_hCost(start_position, neighbour_cells[i]) < smallest_distance && neighbour_cells[i].get_blocked() == false)
                    {   
                        smallest_distance = determine_hCost(start_position, neighbour_cells[i]);
                        return_cell = neighbour_cells[i];
                    }
                }
            }
            return return_cell;
        }

    public GridCell get_nearest_cabinet_neighbour_cell(int start_x, int start_y, int x_position, int y_position)
    {
        return get_nearest_cabinet_neighbour_cell(start_x, start_y, x_position, y_position, get_cabinet_side(x_position, y_position));
    }

    public Grid<GridCell> GetGrid()
    {
        return grid;
    }
    
}
