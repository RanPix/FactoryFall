using System.Collections.Generic;
using UnityEngine;

public class PlacedObject : MonoBehaviour
{
    public static PlacedObject Create(Vector3 worldPosition, Vector3Int origin, PlacedBlockType.Dir dir, PlacedBlockType placedBlockType)
    {
        Transform placedObjectInstance = Instantiate(placedBlockType.Prefab, worldPosition, Quaternion.Euler(0, placedBlockType.GetRotationAngle(dir), 0));
        
        PlacedObject placedObject = placedObjectInstance.GetComponent<PlacedObject>();

        placedObject.placedBlockType = placedBlockType;
        placedObject.origin = origin;
        placedObject.dir = dir;

        return placedObject;
    }

    private PlacedBlockType placedBlockType;
    private Vector3Int origin;
    private PlacedBlockType.Dir dir;

    public List<Vector3Int> GetGridPositionList()
    {
        return placedBlockType.GetGridPositionList(origin, dir);
    }

    //public void DestroySelf()
    //{
    //    Destroy(gameObject);
    //}


}
