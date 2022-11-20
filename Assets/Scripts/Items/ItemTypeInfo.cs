using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemSystem;

[CreateAssetMenu(fileName = "Item Type Info", menuName = "ItemTypeInfo")]
public class ItemTypeInfo : ScriptableObject
{
    public ItemType itemType;

    public int maxStack;
    
    public string itemName;
    public string description;

    public Sprite icon;
}