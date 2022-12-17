using UnityEngine;
using Mirror;

public class GamePlayer : NetworkBehaviour, IDamagable
{
    [SerializeField] private Health health;

    [SerializeField] private InventoryUI inventory;

    [field: SerializeField]public GameObject cameraHolder { get; private set; }
    [Header("Camera")] 

    [SerializeField] private Transform cameraPosition;
    [SerializeField] private Transform orientation;
    private void Start()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        cameraHolder = Instantiate(cameraHolder);
        cameraHolder.GetComponent<MoveCamera>().cameraPosition = cameraPosition;
        cameraHolder.GetComponent<Look>().orientation = orientation;
        cameraHolder.GetComponent<Look>().inventoryUI = inventory;
        cameraHolder.GetComponent<Look>()._isLocalPlayer = true;

        /*if (!isLocalPlayer)
        {
            cameraHolder.GetComponentInChildren<Camera>().enabled = false;
            cameraHolder.GetComponentInChildren<Camera>().gameObject.GetComponentInChildren<Camera>().enabled = false;

        }*/
           
        //NetworkServer.Spawn(cameraHolder);
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

    public bool Damage(float damage)
    {
        health.currentHealth -= damage;
        return false;
    }
}
