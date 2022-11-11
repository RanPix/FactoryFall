using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingGrid : MonoBehaviour
{
    public Grid<GridObject> grid;

    private void Awake()
    {
        int gridWidth = 30;
        int gridHeight = 10;
        int gridLength = 30;

        float cellSize = 3f;

        grid = new Grid<GridObject>(gridWidth, gridHeight, gridLength, cellSize, new Vector3(-50, 0, -50), (Grid<GridObject> g, int x, int y, int z) => new GridObject(g, x, y, z));
    }

    void Update()
    {
    }

    
}

public class GridObject
{
    private Grid<GridObject> grid;
    private int x;
    private int y;
    private int z;

    private Transform transform;

    public GridObject(Grid<GridObject> grid, int x, int y, int z)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public void SetTransform(Transform transform)
    {
        this.transform = transform;
        grid.TriggerGridObjectChanged(x, y, z);
    }

    public void SetTransform()
    {
        transform = null;
        grid.TriggerGridObjectChanged(x, y, z);
    }

    public bool CanBuild()
    {
        return transform == null;
    }

    public override string ToString()
    {
        return $"{x}, {y}, {z} \n {transform}";
    }
}