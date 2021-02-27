using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

/// <summary>
/// This class manages the pathfinding
/// </summary>
public class PathManager : MonoBehaviour
{

    private AstarPathfinder pathfinding;
    public Camera camera;
    private Transform transform;
    private CharacterController moveControllerPathfinding;
    private List<GridCell> path = new List<GridCell>();
    int iterator;
    bool moving;
    private int layerMask = 1 << 10;

    // Start is called before the first frame update
    void Start()
    {
        pathfinding = new AstarPathfinder(30, 20);
        transform = GetComponent<Transform>();
        moveControllerPathfinding = this.transform.GetComponent<CharacterController>();
        moving = false;
        iterator = 0;

        //set the furniture positions of the given scene blocked
        Vector2[] furnitureCellsArray = { new Vector2(7,0),
        new Vector2(8,0),
        new Vector2(7,1),
        new Vector2(9,1),
        new Vector2(7,2),
        new Vector2(8,2),
        new Vector2(7,3),
        new Vector2(8,3),
        new Vector2(7,4),
        new Vector2(8,4),
        new Vector2(7,9),
        new Vector2(8,9),
        new Vector2(7,10),
        new Vector2(8,10),
        new Vector2(7,11),
        new Vector2(8,11),
        new Vector2(7,12),
        new Vector2(8,12),
        new Vector2(7,13),
        new Vector2(8,13),
        new Vector2(3,17),
        new Vector2(4,17),
        new Vector2(3,18),
        new Vector2(4,18),
        new Vector2(3,19),
        new Vector2(4,19),
        new Vector2(10,0),
        new Vector2(11,0),
        new Vector2(12,0),
        new Vector2(10,1),
        new Vector2(11,1),
        new Vector2(12,1),
        new Vector2(9,4),
        new Vector2(9,5),
        new Vector2(9,6),
        new Vector2(9,7),
        new Vector2(9,8),
        new Vector2(9,9),
        new Vector2(9,10),
        new Vector2(9,11),
        new Vector2(9,12),
        new Vector2(9,13),
        new Vector2(10,4),
        new Vector2(10,5),
        new Vector2(10,6),
        new Vector2(10,7),
        new Vector2(10,8),
        new Vector2(10,9),
        new Vector2(10,10),
        new Vector2(10,11),
        new Vector2(10,12),
        new Vector2(10,13),
        new Vector2(27,1),
        new Vector2(28,1),
        //new Vector2(29,1),
        new Vector2(19,4),
        new Vector2(19,5),
        new Vector2(19,6),
        new Vector2(19,7),
        new Vector2(19,8),
        new Vector2(19,9),
        new Vector2(19,10),
        new Vector2(20,4),
        new Vector2(20,5),
        new Vector2(20,6),
        new Vector2(20,7),
        new Vector2(20,8),
        new Vector2(20,9),
        new Vector2(20,10),
        new Vector2(22,5),
        new Vector2(22,6),
        new Vector2(22,7),
        new Vector2(23,5),
        new Vector2(23,6),
        new Vector2(23,7),
        new Vector2(18,17),
        new Vector2(19,17),
        new Vector2(19,18),
        new Vector2(20,18),
        new Vector2(28,9),
        new Vector2(28,10),
        new Vector2(28,11),
        new Vector2(28,12),
        new Vector2(28,13),
        new Vector2(28,14),
        new Vector2(28,15),
        new Vector2(28,16),
        new Vector2(28,17),
        new Vector2(28,18),
        new Vector2(29,9),
        new Vector2(29,10),
        new Vector2(29,11),
        new Vector2(29,12),
        new Vector2(29,13),
        new Vector2(29,14),
        new Vector2(29,15),
        new Vector2(29,16),
        new Vector2(29,17),
        new Vector2(29,18)};
        List <Vector2> furnitureCells = new List<Vector2> (furnitureCellsArray);
        foreach (Vector2 element in furnitureCells)
        {
            int x = (int) element[0];
            int y = (int) element[1];
            pathfinding.GetGrid().getValue(x, y).set_blocked();
            pathfinding.GetGrid().getValue(x, y).draw_blocked(5f);
        }
        //create a raysource for the given scene
        pathfinding.create_raySource(19, 9);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //if the mouse is leftclicked, cast a ray in the center of the camera to set the target point for the pathplanning
            RaycastHit hit;
            Ray ray = camera.ViewportPointToRay(new Vector3(0.5f, 0.5f));
            Vector3 hitpoint = new Vector3();

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                hitpoint = hit.point;
                Debug.DrawLine(hitpoint, hitpoint + new Vector3(0, 1, 0),Color.blue ,3f);
            }


            //stop, if the ray didn't hit the floor
            if (hit.collider == null)
            {
                return;
            }

            //determine startcell and endcell
            int goal_x;
            int goal_z;

            pathfinding.GetGrid().findCell(hitpoint, out goal_x, out goal_z);


            int current_x;
            int current_z;
            pathfinding.GetGrid().findCell(transform.position, out current_x, out current_z);

            //Find the path
            //List<GridCell> path = new List<GridCell>();
            this.path = pathfinding.find_path(current_x, current_z, goal_x, goal_z);
            moving = true;
            //Draw the path
            for (int i = 0; i < path.Count - 1; i++)
            {
                Debug.DrawLine(new Vector3(path[i].get_x(), 0, path[i].get_y()) * pathfinding.GetGrid().getCellSize() + (new Vector3(1f, 0f, 1f) * pathfinding.GetGrid().getCellSize() * 0.5f) + (pathfinding.GetGrid().getOffset()),
                              (new Vector3(path[i + 1].get_x(), 0, path[i + 1].get_y()) * pathfinding.GetGrid().getCellSize()) + (new Vector3(1f, 0f, 1f) * pathfinding.GetGrid().getCellSize() * 0.5f) + (pathfinding.GetGrid().getOffset()),
                               Color.red,
                               5f);
            }
        }

        //move the character
        if(moving == true)
        {
            //determine the offset to the next point in the path
            float x = path[iterator].get_x() * pathfinding.GetGrid().getCellSize() + pathfinding.GetGrid().getOffset().x + 0.5f - transform.position.x;
            float z = path[iterator].get_y() * pathfinding.GetGrid().getCellSize() + pathfinding.GetGrid().getOffset().z + 0.5f - transform.position.z;
            Vector3 offset = new Vector3(x, 0, z);
            
            //move to the direction of the offset until the offset falls below a distance-threshold 
            if (offset.magnitude > .1f)
            {
                //Debug.Log(offset);
                offset = offset.normalized * 7f * Time.deltaTime;
                //Debug.Log(offset);
                moveControllerPathfinding.Move(offset);
            }
            //if the point in the path is reached, set the next point as a target
            else
            {
                iterator++;
            }

            //if the character is at the end of the path, stop and reset the path iterator
            if(iterator == path.Count)
            {
                iterator = 0;
                moving = false;
            }
        }
    }
}
