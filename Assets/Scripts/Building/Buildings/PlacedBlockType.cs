using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class PlacedBlockType : ScriptableObject
{
    public static Dir GetNextDir(Dir dir)
    {
        switch (dir)
        {
            default:
            case Dir.Backward:  return Dir.Left;
            case Dir.Left:      return Dir.Forward;
            case Dir.Forward:   return Dir.Right;
            case Dir.Right:     return Dir.Backward;
        }
    }

    public enum Dir
    {
        Backward,
        Forward,

        Left,
        Right,
    }

    [SerializeField] private string blockName;

    [SerializeField] private Transform prefab;
    [SerializeField] private Transform visual;

    [SerializeField] private int gridWidth;
    [SerializeField] private int gridHeight;
    [SerializeField] private int gridLength;

    [field: SerializeField] public bool isSupport { get; private set; }

    public string BlockName => blockName;

    public Transform Prefab => prefab;
    public Transform Visual => visual;

    public int GridWidth => gridWidth;
    public int GridHeight => gridHeight;
    public int GridLength => gridLength;


    public int GetRotationAngle(Dir dir)
    {
        switch (dir)
        {
            default:
            case Dir.Backward:  return 0;
            case Dir.Left:      return 90;
            case Dir.Forward:   return 180;
            case Dir.Right:     return 270;
        }
    }

    public static int GetRotationAngleStatic(Dir dir)
    {
        switch (dir)
        {
            default:
            case Dir.Backward: return 0;
            case Dir.Left: return 90;
            case Dir.Forward: return 180;
            case Dir.Right: return 270;
        }
    }

    public Vector2Int GetRotationOffset(Dir dir)
    {
        switch (dir)
        {
            default:
            case Dir.Backward: return new Vector2Int(0, 0);
            case Dir.Left:     return new Vector2Int(0, gridWidth);
            case Dir.Forward:  return new Vector2Int(gridWidth, gridLength);
            case Dir.Right:    return new Vector2Int(gridLength, 0);
        }
    }

    public List<Vector3Int> GetGridPositionList(Vector3Int offset, Dir dir)
    {
        List<Vector3Int> positionList = new List<Vector3Int>();

        switch (dir)
        {
            default:
            case Dir.Backward:
            case Dir.Forward:
                for (int x = 0; x < gridWidth; x++)
                {
                    for (int y = 0; y < gridHeight; y++)
                    {
                        for (int z = 0; z < gridLength; z++)
                        {
                            positionList.Add(offset + new Vector3Int(x, y, z));
                        }
                    }
                }
                break;

            case Dir.Left:
            case Dir.Right:
                for (int x = 0; x < gridLength; x++)
                {
                    for (int y = 0; y < gridHeight; y++)
                    {
                        for (int z = 0; z < gridWidth; z++)
                        {
                            positionList.Add(offset + new Vector3Int(x, y, z));
                        }
                    }
                }
                break;
        }

        return positionList;
    }

    public List<Vector3Int> GetBottomGridPositionsList(Vector3Int offset, Dir dir)
    {
        List<Vector3Int> positionList = new List<Vector3Int>();

        switch (dir)
        {
            default:
            case Dir.Backward:
            case Dir.Forward:
                for (int x = 0; x < gridWidth; x++)
                {
                    for (int z = 0; z < gridLength; z++)
                    {
                        positionList.Add(offset + new Vector3Int(x, 0, z));
                    }
                }
                break;

            case Dir.Left:
            case Dir.Right:
                for (int x = 0; x < gridLength; x++)
                {
                    for (int z = 0; z < gridWidth; z++)
                    {
                        positionList.Add(offset + new Vector3Int(x, 0, z));
                    }
                }
                break;
        }

        return positionList;
    }
}
