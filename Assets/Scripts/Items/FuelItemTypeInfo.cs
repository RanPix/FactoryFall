using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemSystem;

[CreateAssetMenu(fileName = "Fuel Item Type Info", menuName = "FuelItemTypeInfo")]
public class FuelItemTypeInfo : ItemTypeInfo
{
    [SerializeField] private int fuelValue;
    public int FuelValue => fuelValue;
}
