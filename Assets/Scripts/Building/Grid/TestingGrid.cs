using UnityEngine;

public class TestingGrid : MonoBehaviour
{
    public Grid<GridObject> grid;

    private void Awake()
    {
        int gridWidth = 25;
        int gridHeight = 6;
        int gridLength = 25;

        float cellSize = 3f;

        grid = new Grid<GridObject>(gridWidth, gridHeight, gridLength, cellSize, new Vector3(-50, 0, -50), (Grid<GridObject> g, int x, int y, int z) => new GridObject(g, x, y, z));
    }
}