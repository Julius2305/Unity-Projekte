using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

/// <summary>
/// This class manages the pathfinding and the data generation
/// </summary>
public class PathfindingManager : MonoBehaviour
{

    private AstarPathfinder pathfinding;
    public int number_of_cabinets = 1;
    private List<int[]> cabinet_centers = new List<int[]>();

    // Start is called before the first frame update
    void Start()
    {
        pathfinding = new AstarPathfinder(10, 10);

        generate_data(200);


        //The cabinets are a feature that was developed in an earlier phase of the project, but later discarded. The idea was
        //to create a lot of paths to random positioned cabinets to learn an optimal positioning of the furniture
        for (int i = 0; i < number_of_cabinets; i++)
        {
            new_cabinet();
        }

        //Finds the path(s) to the created cabinet(s)
        for (int i = 0; i < number_of_cabinets; i++)
        {
            find_path_to_cabinet(i);
        }

    }

    // Update is called once per frame
    void Update()
    {
        //tests, if the mouse is leftclicked
        //if the mouse gets leftclicked, a path will be generated from the cell at grid-position (0,0) to the cell at the current mouse position. 
        //This was implemented to show, that the pathfinding works
        if (Input.GetMouseButtonDown(0))
        {
            //if the mouse is leftclicked, the current mouse position gets determined
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);        //the current mouse position

            //determine the destination for the path
            int x;
            int y;
            pathfinding.GetGrid().findCell(mousePosition, out x, out y);
            

            //Find a path from the cell at grid-position (0,0) to the destination
            List<GridCell> path = new List<GridCell>();
            path = pathfinding.find_path(0, 0, x, y);

            //Draw the path
            for(int i = 0; i < path.Count - 1; i++)
            {
                Debug.DrawLine(new Vector3(path[i].get_x(), path[i].get_y()) * pathfinding.GetGrid().getCellSize() + (Vector3.one * pathfinding.GetGrid().getCellSize() * 0.5f),
                              (new Vector3(path[i + 1].get_x(), path[i + 1].get_y()) * pathfinding.GetGrid().getCellSize()) + (Vector3.one * pathfinding.GetGrid().getCellSize() * 0.5f),
                               Color.red,
                               5f);
            }
        }

        //tests, if the mouse is rightclicked
        //if the mouse gets rightclicked, the cell at the current mouse position will get blocked/unblocked for the pathfinding. 
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            int x;
            int y;
            pathfinding.GetGrid().findCell(mousePosition, out x, out y);
            pathfinding.GetGrid().getValue(x, y).set_blocked();
            pathfinding.GetGrid().getValue(x, y).draw_blocked(5f);
        }

        //tests, if the space key is pressed
        //if the space key is pressed, the cell at the current mouse position will become a (theoretical) ray source and will get blocked. Only one ray source can exist at the same time, old ray sources will get deactivated by this
        if (Input.GetKeyDown("space"))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            int x;
            int y;
            pathfinding.GetGrid().findCell(mousePosition, out x, out y);
            pathfinding.create_raySource(x, y);
        }
    }

    /// <summary>
    /// Creates data for neural networks and saves it in a textfile. A given number of samples will be generated.
    /// The data is saved as a "weightmatrix; start- and endpositions of the path; path; position of the ray source"
    /// </summary>
    public void generate_data(int count)
    {
        for (int i = 0; i < count; i++)
        {
            //initialize the pathfinding
            pathfinding = new AstarPathfinder(10, 10);
            //find random start- and endpositions for a path and a ray source
            int ray_source_x_position = Random.Range(0, pathfinding.GetGrid().getWidth());
            int ray_source_y_position = Random.Range(0, pathfinding.GetGrid().getHeight());

            int random_start_x_position = Random.Range(0, pathfinding.GetGrid().getWidth());
            int random_start_y_position = Random.Range(0, pathfinding.GetGrid().getHeight());
            int random_end_x_position = Random.Range(0, pathfinding.GetGrid().getWidth());
            int random_end_y_position = Random.Range(0, pathfinding.GetGrid().getHeight());
            Grid<GridCell> grid = pathfinding.GetGrid();
            string data = "";
            //assure, ray source position is not equal to start or end of the path
            while ((random_start_x_position == ray_source_x_position && random_start_y_position == ray_source_y_position) || (random_end_x_position == ray_source_x_position && random_end_y_position == ray_source_y_position))
            {
                ray_source_x_position = Random.Range(0, pathfinding.GetGrid().getWidth());
                ray_source_y_position = Random.Range(0, pathfinding.GetGrid().getHeight());
            }
            //create ray source
            pathfinding.create_raySource(ray_source_x_position, ray_source_y_position);

            //find the path
            List<GridCell> path = new List<GridCell>();
            path = pathfinding.find_path(random_start_x_position, random_start_y_position, random_end_x_position, random_end_y_position);

            //save weightmatrix
            for (int x = 0; x < grid.getWidth(); x++)
            {
                for (int y = 0; y < grid.getHeight(); y++)
                {
                    GridCell gridCell = grid.getValue(x, y);
                    data = data + ((int)(gridCell.get_hCost() + gridCell.get_additionalCost())).ToString() + ", ";
                }
            }
            data = data + ";";
            //save the start- and endpoints
            data = data + "[" + random_start_x_position.ToString() + "," + random_start_y_position.ToString() + "] " + "[" + random_end_x_position.ToString() + "," + random_end_y_position.ToString() + "];";
            //save the path
            for (int j = 0; j < path.Count - 1; j++)
            {
                data = data + "(" + path[j].get_x().ToString() + "," + path[j].get_y().ToString() + "), ";
            }

            data = data + ";";
            data = data + ray_source_x_position.ToString() + "," + ray_source_y_position.ToString();

            //write all data into a file at the location "file_path"
            string file_path = "Assets/Resources/200_training_data.txt";
            //Write some text to the data.txt file
            StreamWriter writer = new StreamWriter(file_path, true);
            writer.WriteLine(data);
            writer.Close();
        }
    }

    /// <summary>
    /// Manages the creation of a cabinet at a random position at the border of the current grid
    /// </summary>
    public void new_cabinet()
    {
        //Random furniture position in the edge. The cabinets have a width of 3 and a depth of 1
        //0 = down, 1 = right, 2 = up, 3 = left
        int rand = Random.Range(0, 4);
        int x_position;
        int y_position;
        
        //find a random position at the given border of the grid
        switch (rand)
        {
            case 0:
                x_position = Random.Range(1, pathfinding.GetGrid().getWidth() - 1);
                y_position = 0;
                break;
            case 1:
                y_position = Random.Range(1, pathfinding.GetGrid().getHeight() - 1);
                x_position = pathfinding.GetGrid().getWidth() - 1;
                break;
            case 2:
                x_position = Random.Range(1, pathfinding.GetGrid().getWidth() - 1);
                y_position = pathfinding.GetGrid().getHeight() - 1;
                break;
            case 3:
                y_position = Random.Range(1, pathfinding.GetGrid().getHeight() - 1);
                x_position = 0;
                break;
            default:
                x_position = 0;
                y_position = 0;
                break;
        }
        
        //test, if the position is available (not blocked or direct neighbor to another cabinet)        
        while(pathfinding.GetGrid().getValue(x_position, y_position).get_furniture_availability() == false)
        {
            switch (rand)
            {
                case 0:
                    x_position = Random.Range(1, pathfinding.GetGrid().getWidth() - 1);
                    y_position = 0;
                    break;
                case 1:
                    y_position = Random.Range(1, pathfinding.GetGrid().getHeight() - 1);
                    x_position = pathfinding.GetGrid().getWidth() - 1;
                    break;
                case 2:
                    x_position = Random.Range(1, pathfinding.GetGrid().getWidth() - 1);
                    y_position = pathfinding.GetGrid().getHeight() - 1;
                    break;
                case 3:
                    y_position = Random.Range(1, pathfinding.GetGrid().getHeight() - 1);
                    x_position = 0;
                    break;
                default:
                    x_position = 0;
                    y_position = 0;
                    break;
            }
        }

        //create the cabinet
        int[] position = {x_position, y_position };
        cabinet_centers.Add(position);
        pathfinding.create_cabinet(x_position, y_position, rand);
    }

    /// <summary>
    /// Finds a path from (0, 0) to the cell, nearest to (0, 0) and neighbour to a given cabinet
    /// </summary>
    public void find_path_to_cabinet(int cabinet)
    {
        if(cabinet < cabinet_centers.Count)
        {
            //get the neighbour cell of the given cabinet, to which a path is to be found
            GridCell nearest_cell = pathfinding.get_nearest_cabinet_neighbour_cell(0, 0, cabinet_centers[cabinet][0], cabinet_centers[cabinet][1]);
            //Find the path (from (0, 0))
            List<GridCell> path = new List<GridCell>();
            path = pathfinding.find_path(0, 0, nearest_cell.get_x(), nearest_cell.get_y());

            //Draw the path
            for (int i = 0; i < path.Count - 1; i++)
            {
                Debug.DrawLine(new Vector3(path[i].get_x(), path[i].get_y()) * pathfinding.GetGrid().getCellSize() + (Vector3.one * pathfinding.GetGrid().getCellSize() * 0.5f),
                              (new Vector3(path[i + 1].get_x(), path[i + 1].get_y()) * pathfinding.GetGrid().getCellSize()) + (Vector3.one * pathfinding.GetGrid().getCellSize() * 0.5f),
                               Color.red,
                               50f);
            }
        }
    }
}
