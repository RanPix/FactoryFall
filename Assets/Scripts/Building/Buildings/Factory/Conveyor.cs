using Mirror;
using System.Collections.Generic;
using UnityEngine;

public class Conveyor : NetworkBehaviour
{
    private PlacedBlockType.Dir conveyorDirection;

    [SerializeField] private float transportSpeed;
    [SerializeField] private float itemSpacing;
    [SerializeField] private int maxCapacity;

    [SerializeField] private Transform itemStartPos;
    [SerializeField] private Transform itemEndPos;

    [SerializeField] private Transform item;

    [SerializeField] private Transform[] items;

    private void Awake()
    {
        item = Instantiate(item, itemStartPos.position, Quaternion.identity, transform);

        conveyorDirection = GetComponent<PlacedObject>().dir;
    }

    private void FixedUpdate()
    {
        MoveItems();    
    }

    private void MoveItems()
    {
        item.position = Vector3.MoveTowards(item.position, itemEndPos.position, transportSpeed);
    }
}
