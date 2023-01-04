using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBuilding : MonoBehaviour
{
    public static PlayerBuilding Instance { get; private set; }

    private PlayerControls controls;

    [SerializeField] private LayerMask groundLM;
    private RaycastHit removingBlockHit;
    private RaycastHit blockHit;

    private float removingTimer;
    private Block removedBlock;

    [SerializeField] private float buildDistance;
    private Vector3 buildingPosition;

    private Grid<GridObject> gridRef;

    [SerializeField] private PlacedBlockType[] buildings;
    public int buildingType { get; private set; }
    private PlacedBlockType.Dir blockDir;

    [HideInInspector] public Action<PlacedBlockType> OnSelectedBlockChanged;
    [HideInInspector] public Action OnBuildingDisabled;
    public bool canPlaceBuilding { get; private set; }

    [SerializeField] private GameObject buildingGhost;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        else
            Destroy(gameObject);

        Instantiate(buildingGhost);
    }

    private void Start()
    {
        gridRef = GameObject.Find("Grid").GetComponent<TestingGrid>().grid;

        controls = new PlayerControls();
        controls.Player.Enable();

        controls.Player.PickBlock.performed += GetBlockIndex;
        controls.Player.RotateBlock.performed += RotateBlock;
        controls.Player.PlaceBlock.performed += PlaceBlock;
    }

    private void Update()
    {
        buildingPosition = GetBuildingPosition();

        RemoveBlock();
    }

    private void GetBlockIndex(InputAction.CallbackContext context)
    {
        int newIndex = Mathf.RoundToInt(context.ReadValue<float>());

        buildingType = newIndex == buildingType ? -1 : newIndex;

        OnSelectedBlockChanged?.Invoke(buildingType == -1 ? null : buildings[buildingType]);
    }

    private void RotateBlock(InputAction.CallbackContext context)
        => blockDir = PlacedBlockType.GetNextDir(blockDir);

    public Quaternion GetBuildingRotation()
    {
        if (buildingType == -1)
            return Quaternion.Euler(new Vector3(0, PlacedBlockType.GetRotationAngleStatic(blockDir), 0));

        if (buildings[buildingType] != null)
            return Quaternion.Euler(new Vector3(0, buildings[buildingType].GetRotationAngle(blockDir), 0));
        else
            return Quaternion.identity;
    }

    private Vector3 GetBuildingPosition()
    {
        Vector3 placeBlockPosition;

        if (Physics.Raycast(transform.position, transform.forward, out blockHit, buildDistance))
            placeBlockPosition = blockHit.point;

        else
            placeBlockPosition = transform.position + (transform.forward * buildDistance);

        return placeBlockPosition;
    }

    public Vector3 GetWorldSnappedBuildingPosition()
    {
        if (buildingType == -1)
            return buildingPosition;

        gridRef.GetXYZ(buildingPosition, out int x, out int y, out int z);

        Vector2Int rotationOffset = buildings[buildingType].GetRotationOffset(blockDir);
        Vector3 placedObjectWorldPosition = gridRef.GetWorldPosition(x, y, z) + new Vector3(rotationOffset.x, 0, rotationOffset.y) * gridRef.cellSize;

        //placedObjectWorldPosition -= new Vector3(buildings[buildingType].GridWidth, buildings[buildingType].GridHeight, buildings[buildingType].GridLength) * 0.5f;

        return placedObjectWorldPosition;
    }

    private void PlaceBlock(InputAction.CallbackContext context)
    {
        if (buildingType == -1)
            return;

        gridRef.GetXYZ(buildingPosition, out int x, out int y, out int z);

        List<Vector3Int> positionsList = buildings[buildingType].GetGridPositionList(new Vector3Int(x, y, z), blockDir);

        bool canBuild = true;
        foreach (Vector3Int position in positionsList)
        {
            bool isNullPosition = gridRef.GetGridObject(position.x, position.y, position.z) == null;

            if (isNullPosition || !gridRef.GetGridObject(position.x, position.y, position.z).CanBuild())
            {
                canBuild = false;
                break;
            }
        }

        if (!canBuild)
            return;

        bool floorDetected = Physics.Raycast(buildingPosition, Vector3.down, gridRef.cellSize - 0.1f, groundLM, QueryTriggerInteraction.Ignore);
        if (floorDetected)
            canBuild = true;


        if (!floorDetected)
        {
            List<Vector3Int> bottomPositionsList = buildings[buildingType].GetBottomGridPositionsList(new Vector3Int(x, y, z), blockDir);

            foreach (Vector3Int position in bottomPositionsList)
            {
                PlacedObject bottomBlock = gridRef.GetGridObject(position.x, Math.Clamp(y - 1, 0, gridRef.height), position.z).GetPlacedObject();

                if (bottomBlock == null)
                {
                    canBuild = false;
                    break;
                }

                if (!bottomBlock.isSupport)
                {
                    canBuild = false;
                    break;
                }
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
            GridObject gridObject = gridRef.GetGridObject(buildingPosition);

            if (gridObject == null)
                return;

            PlacedObject placedObject = gridObject.GetPlacedObject();

            if (placedObject != null)
            {
                List<Vector3Int> positionsList = placedObject.GetGridPositionList();

                foreach (Vector3Int position in positionsList)
                    gridRef.GetGridObject(position.x, position.y, position.z).ClearPlacedObject();

                //Destroy(placedObject);
                placedObject.DestroySelf();
            }
        }

        return;

        if (controls.Player.RemoveBlock.IsPressed())
        {
            bool blockHit = Physics.Raycast(transform.position, transform.forward, out removingBlockHit, buildDistance);

            if (removingBlockHit.transform == null)
            {
                removedBlock = null;
                return;
            }
            else if (removingBlockHit.transform.GetComponent<Block>() == null)
                return;

            if (blockHit)
            {
                if (removedBlock == null || removedBlock != removingBlockHit.transform.GetComponent<Block>())
                {
                    removedBlock = removingBlockHit.transform.GetComponent<Block>();
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
        }
        */
    }

    //private void OnDrawGizmos()
    //{
    //   Gizmos.DrawWireSphere(buildingPosition, 0.3f);
    //}
}
