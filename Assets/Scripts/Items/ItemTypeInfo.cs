using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item Type Info", menuName = "ItemTypeInfo")]
public class ItemTypeInfo : ScriptableObject
{
    public ItemType itemType;

    public string itemName;
    public string description;

    public Sprite icon;
}