using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    public static PlayerInteraction instance;
    public GamePlayer player;

    [SerializeField] private float interactionDistance;

    private PlayerControls controls;
    private RaycastHit interact;

    private float removingTimer;
    private Block removedBlock;

    [SerializeField] private float dmg; // test field
    [SerializeField] private GameObject furnace; // test field

    [SerializeField] private float buildDistance;
    private Grid<GridObject> gridRef;

    [SerializeField] private GameObject[] buildings;
    private int buildingType;

    private void Start()
    {
        gridRef = GameObject.Find("Grid").GetComponent<TestingGrid>().grid;

        instance = this;

        controls = new PlayerControls();
        controls.Player.Enable();

        controls.Player.PickBlock.performed += GetInput;
    }

    private void Update()
    {
        CheckInteraction();
    }

    private void GetInput(InputAction.CallbackContext context)
    {
        float buildingIndexf = controls.Player.PickBlock.ReadValue<float>();
        buildingType = Mathf.RoundToInt(buildingIndexf);
    }

    private void CheckInteraction() 
    {
        if (controls.Player.RemoveBlock.IsPressed())
        {
            bool blockHit = Physics.Raycast(transform.position, transform.forward, out interact, interactionDistance);

            if (interact.transform == null)
            {
                removedBlock = null;
                return;
            }
            else if (interact.transform.GetComponent<Block>() == null)
                return;

            if (blockHit)
            {
                if (removedBlock == null || removedBlock != interact.transform.GetComponent<Block>())
                {
                    removedBlock = interact.transform.GetComponent<Block>();
                    removingTimer = 0f;
                }

                removingTimer += Time.deltaTime;

                if (removingTimer > removedBlock.GetRemoveTime())
                {
                    //player.RemoveBlock(removedBlock);
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

        else if (controls.Player.Interact.WasPerformedThisFrame())
        {
            //if (Physics.Raycast(transform.position, transform.forward, out interact, interactionDistance))
            //    player.Interact(interact.transform.GetComponent<IInteractable>());
        }

        else if (controls.Player.PlaceBlock.WasPerformedThisFrame())
        {
            gridRef.GetXYZ(transform.position + (transform.forward * buildDistance), out int x, out int y, out int z);

            GridObject gridObject = gridRef.GetGridObject(x, y, z);

            if (gridObject.CanBuild())
            {
                Transform buildingTransform = Instantiate(buildings[buildingType], gridRef.GetWorldPosition(x, y, z), Quaternion.identity).transform;
                gridObject.SetTransform(buildingTransform);
            }
            else
                print("cant build here");
        }
    }
}
