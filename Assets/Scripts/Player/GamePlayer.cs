using UnityEngine;
using Mirror;
using GameBase;
using Player.Info;

namespace Player
{
    public class GamePlayer : NetworkBehaviour
    {
        private PlayerInfo playerInfo;

        [SerializeField] private Health health;
        [SerializeField] private Damage damage;

        [SerializeField] private InventoryUI inventory;

    [SerializeField] private GameObject cameraHolder;
    [SerializeField] private Transform cameraPosition;
    [SerializeField] private Transform orientation;
    private void Start()
    {
        if (isLocalPlayer)
        {
            cameraHolder = Instantiate(cameraHolder);
            cameraHolder.GetComponent<MoveCamera>().cameraPosition = cameraPosition;
            cameraHolder.GetComponent<Look>().orientation = orientation;
            cameraHolder.GetComponent<Look>().inventoryUI = inventory;
            cameraHolder.GetComponent<Look>()._isLocalPlayer = true;
            NetworkServer.Spawn(cameraHolder);
        }
        else
        {
            cameraHolder.GetComponentInChildren<AudioListener>().enabled = false;
        }
        /*if (!isLocalPlayer)
        {
            cameraHolder.GetComponentInChildren<Camera>().enabled = false;
            cameraHolder.GetComponentInChildren<Camera>().gameObject.GetComponentInChildren<Camera>().enabled = false;

        }*/
           
        //NetworkServer.Spawn(cameraHolder);
        //PlayerInteraction.instance.player = this;
    }

    [Command]
    public void SpawnOnServer()
    {
        NetworkServer.Spawn(cameraHolder);
    }

    /*public void Interact(IInteractable interact) =>
        InteractServer(interact);

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
}