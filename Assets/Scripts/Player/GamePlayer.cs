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

            cameraHolder.GetComponentInChildren<Look>().inventoryUI = inventory;

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