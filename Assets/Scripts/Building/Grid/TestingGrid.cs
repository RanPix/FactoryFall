using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingGrid : MonoBehaviour
{
    public Grid<GridObject> grid;

    private void Awake()
    {
        int gridWidth = 100;
        int gridHeight = 10;
        int gridLength = 100;

        float cellSize = 3f;

        grid = new Grid<GridObject>(gridWidth, gridHeight, gridLength, cellSize, new Vector3(-50, 0, -50), (Grid<GridObject> g, int x, int y, int z) => new GridObject(g, x, y, z));
    }

    void Update()
    {
    }

    
}