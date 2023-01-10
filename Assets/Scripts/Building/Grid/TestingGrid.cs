using UnityEngine;

public class TestingGrid : MonoBehaviour
{
    public Grid3D<GridObject> grid;

    private void Awake()
    {
        int gridWidth = 25;
        int gridHeight = 6;
        int gridLength = 25;

        float cellSize = 3f;

        grid = new Grid3D<GridObject>(gridWidth, gridHeight, gridLength, cellSize, new Vector3(-50, 0, -50), (Grid3D<GridObject> g, int x, int y, int z) => new GridObject(g, x, y, z));
    }
}