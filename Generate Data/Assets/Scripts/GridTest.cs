using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// In this class the Grid calss was tested
/// </summary>
public class GridTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //Grid<int> grid = new Grid<int>(10, 5, 10f, new Vector3(0, 0));
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);        //the current mouse position
            Debug.Log("x:" + mousePosition.x + " y:" + mousePosition.y);
        }
        
    }
}
