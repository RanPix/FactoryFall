using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemOnFloor : MonoBehaviour
{
    Item item;

    public Item Take()
    {
        return item;
        Destroy(gameObject);
    }
    
    public Item PutItem(Item itemToPut) => item.PutItem(itemToPut);
}