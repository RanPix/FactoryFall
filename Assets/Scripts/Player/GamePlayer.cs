using UnityEngine;
using Mirror;

public class GamePlayer : NetworkBehaviour
{
    //[SerializeField] public GameObject inventory;

    [Header("Camera")]

    [SerializeField] private GameObject cameraHolder;
    [SerializeField] private Transform cameraPosition;
    [SerializeField] private Transform orientation;

    private void Start()
    {
        if (!isLocalPlayer)
            return;

        cameraHolder = Instantiate(cameraHolder);
        cameraHolder.GetComponent<MoveCamera>().cameraPosition = cameraPosition;
        cameraHolder.GetComponentInChildren<Look>().orientation = orientation;

        //acameraHolder.GetComponentInChildren<Look>().inventoryUI = inventory.GetComponent<InventoryUI>();

        //PlayerInteraction.instance.player = this;
    }

    /*public void Interact(IInteractable interact) =>
        InteractServer(interact);

    [Command]
    private void InteractServer(IInteractable interact) =>
        interact.Interact();



    public void RemoveBlock(Block block) =>
        RemoveBlockServer(block);

    [Command]
    private void RemoveBlockServer(Block block) =>
        block.RemoveBlock();*/

}
