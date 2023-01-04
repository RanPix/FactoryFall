using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid<TGritObject>
{
    public event EventHandler<OnGridObjectChangedEventArgs> OnGridObjectChanged;
    public class OnGridObjectChangedEventArgs : EventArgs
    {
        public int x;
        public int y;
        public int z;
    }

    public int width { get; private set; }
    public int height { get; private set; }
    public int length { get; private set; }

    public float cellSize { get; private set; }

    public Vector3 originPos { get; private set; }

    private TGritObject[,,] gridArray;

    public Grid(int width, int height, int length, float cellSize, Vector3 originPos, Func<Grid<TGritObject>, int, int, int, TGritObject> createGridObject)
    {
        this.width = width;
        this.height = height;
        this.length = length;

        this.originPos = originPos;

        this.cellSize = cellSize;

        gridArray = new TGritObject[width, height, length];

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                for (int z = 0; z < gridArray.GetLength(2); z++)
                {
                    gridArray[x, y, z] = createGridObject(this, x, y, z);
                }
            }
        }

        //DebugGrid();
    }

    public Vector3 GetWorldPosition(int x, int y, int z)
        => new Vector3(x, y, z) * cellSize + originPos;

    public void GetXYZ(Vector3 worldPosition, out int x, out int y, out int z)
    {
        x = Mathf.FloorToInt((worldPosition - originPos).x / cellSize);
        y = Mathf.FloorToInt((worldPosition - originPos).y / cellSize);
        z = Mathf.FloorToInt((worldPosition - originPos).z / cellSize);
    }

    private void SetGridObject(int x, int y, int z, TGritObject value)
    {
        if (x >= 0 && y >= 0 && z >= 0 && x < width && y < height && z < length)
        {
            gridArray[x, y, z] = value;

            if (OnGridObjectChanged != null)
                OnGridObjectChanged(this, new OnGridObjectChangedEventArgs { x = x, y = y, z = z });
        }
    }

    public void TriggerGridObjectChanged(int x, int y, int z)
    {
        if (OnGridObjectChanged != null)
            OnGridObjectChanged(this, new OnGridObjectChangedEventArgs { x = x, y = y, z = z });
    }

    public void SetGridObject(Vector3 worldPosition, TGritObject value)
    {
        int x, y, z;
        GetXYZ(worldPosition, out x, out y, out z);
        SetGridObject(x, y, z, value);
    }

    public TGritObject GetGridObject(int x, int y, int z)
    {
        if (x >= 0 && y >= 0 && z >= 0 && x < width && y < height && z < length)
        {
            return gridArray[x, y, z];
        }
        else
        {
            return default(TGritObject);
        }
    }

    public TGritObject GetGridObject(Vector3 worldPosition)
    {
        int x, y, z;
        GetXYZ(worldPosition, out x, out y, out z);
        return GetGridObject(x, y, z);
    }


    private void DebugGrid()
    {
        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                for (int z = 0; z < gridArray.GetLength(2); z++)
                {
                    //Debug.DrawLine(GetWorldPosition(x, y, z), GetWorldPosition(x + 1, y, z), new Color(255, 255, 255, 0.2f), 100f);
                    //Debug.DrawLine(GetWorldPosition(x, y, z), GetWorldPosition(x, y + 1, z), new Color(255, 255, 255, 0.2f), 100f);
                    //Debug.DrawLine(GetWorldPosition(x, y, z), GetWorldPosition(x, y, z + 1), new Color(255, 255, 255, 0.2f), 100f);

                    GridDebugger.CreateWorldText(null, gridArray[x, y, z]?.ToString(), GetWorldPosition(x, y, z) + new Vector3(cellSize, cellSize, cellSize) * 0.5f, 35, 0.1f, Color.white, TextAnchor.MiddleCenter, TextAlignment.Left, 5000);
                }
            }
        }
    }
}


public class GridObject
{
    private Grid<GridObject> grid;
    private int x;
    private int y;
    private int z;

    private PlacedObject placedObject;

    public GridObject(Grid<GridObject> grid, int x, int y, int z)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public void SetPlacedObject(PlacedObject placedObject)
    {
        this.placedObject = placedObject;
    }

    public void SetPlacedObject()
    {
        placedObject = null;
    }

    public PlacedObject GetPlacedObject()
    {
        return placedObject;
    }

    public void ClearPlacedObject()
    {
        placedObject = null;
    }

    public bool CanBuild()
    {
        return placedObject == null;
    }

    public override string ToString()
    {
        return $"{x}, {y}, {z} \n {placedObject}";
    }
}