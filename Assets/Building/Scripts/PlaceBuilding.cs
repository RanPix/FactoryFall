using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceBuilding : MonoBehaviour
{
    public GameObject buildingPrefab;

    public void SetBuildingPrefab(GameObject prefab)
    {
        buildingPrefab = prefab;
    }


}
