using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBuilding : MonoBehaviour
{
    private PlayerControls controls;
    private RaycastHit building;

    private float removingTimer;
    private Block removedBlock;

    [SerializeField] private float buildDistance;
    private Grid<GridObject> gridRef;

    [SerializeField] private PlacedBlockType[] buildings;
    private int buildingType;
    private PlacedBlockType.Dir blockDir;


    void Start()
    {
        gridRef = GameObject.Find("Grid").GetComponent<TestingGrid>().grid;

        controls = new PlayerControls();
        controls.Player.Enable();

        controls.Player.PickBlock.performed += GetBlockIndex;
        controls.Player.RotateBlock.performed += RotateBlock;
        controls.Player.PlaceBlock.performed += PlaceBlock;
    }

    void Update()
    {
        RemoveBlock();
    }

    private void GetBlockIndex(InputAction.CallbackContext context)
        => buildingType = Mathf.RoundToInt(context.ReadValue<float>());

    private void RotateBlock(InputAction.CallbackContext context)
        => blockDir = PlacedBlockType.GetNextDir(blockDir);

    private void PlaceBlock(InputAction.CallbackContext context)
    {
        gridRef.GetXYZ(transform.position + (transform.forward * buildDistance), out int x, out int y, out int z);

        List<Vector3Int> positionsList = buildings[buildingType].GetGridPositionList(new Vector3Int(x, y, z), blockDir);

        bool canBuild = true;
        foreach (Vector3Int position in positionsList)
        {
            bool isNullPosition = gridRef.GetGridObject(position.x, position.y, position.z) == null;
            bool canBuildInThisPosition = !gridRef.GetGridObject(position.x, position.y, position.z).CanBuild();

            if (isNullPosition || canBuildInThisPosition)
            {
                canBuild = false;
                break;
            }
        }

        if (canBuild)
        {
            Vector2Int rotationOffset = buildings[buildingType].GetRotationOffset(blockDir);

            Vector3 placeObjectWorldPosition = gridRef.GetWorldPosition(x, y, z) +
                new Vector3(rotationOffset.x, 0, rotationOffset.y) * gridRef.cellSize;


            PlacedObject placedObject = PlacedObject.Create(placeObjectWorldPosition, new Vector3Int(x, y, z), blockDir, buildings[buildingType]);

            foreach (Vector3Int position in positionsList)
                gridRef.GetGridObject(position.x, position.y, position.z).SetPlacedObject(placedObject);
        }
    }

    private void RemoveBlock()
    {
        if (controls.Player.RemoveBlock.IsPressed())
        {
            if (gridRef.GetGridObject(transform.position + (transform.forward * buildDistance)) == null)
                return;

            GridObject gridObject = gridRef.GetGridObject(transform.position + (transform.forward * buildDistance));
            PlacedObject placedObject = gridObject.GetPlacedObject();

            if (placedObject != null)
            {

                List<Vector3Int> positionsList = placedObject.GetGridPositionList();

                foreach (Vector3Int position in positionsList)
                    gridRef.GetGridObject(position.x, position.y, position.z).ClearPlacedObject();

                Destroy(placedObject);
                //placedObject.DestroySelf();
            }
        }

        return;
        //something. idk what is it but it unreachable

        /*if (controls.Player.RemoveBlock.IsPressed())
        {
            bool blockHit = Physics.Raycast(transform.position, transform.forward, out building, buildDistance);

            if (building.transform == null)
            {
                removedBlock = null;
                return;
            }
            else if (building.transform.GetComponent<Block>() == null)
                return;

            if (blockHit)
            {
                if (removedBlock == null || removedBlock != building.transform.GetComponent<Block>())
                {
                    removedBlock = building.transform.GetComponent<Block>();
                    removingTimer = 0f;
                }

                removingTimer += Time.deltaTime;

                if (removingTimer > removedBlock.GetRemoveTime())
                {
                    Destroy(removedBlock);

                    removingTimer = 0f;
                    removedBlock = null;
                }
            }
        }
        else if (controls.Player.RemoveBlock.WasReleasedThisFrame())
        {
            removingTimer = 0f;
            removedBlock = null;
        }*/
    }
}
