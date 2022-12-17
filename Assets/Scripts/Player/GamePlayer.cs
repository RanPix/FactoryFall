using UnityEngine;
using System.Collections;
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
    [field: SerializeField]public GameObject cameraHolder { get; private set; }

        [SerializeField] private Transform cameraPosition;
        [SerializeField] private Transform orientation;

        private Transform cam;

        private void Start()
        {
            if (isLocalPlayer)
            {

                cameraHolder = Instantiate(cameraHolder);
                cameraHolder.GetComponent<MoveCamera>().cameraPosition = cameraPosition;
                cameraHolder.GetComponent<Look>().orientation = orientation;
                cameraHolder.GetComponent<Look>().inventoryUI = inventory;
                cameraHolder.GetComponent<Look>()._isLocalPlayer = true;

                cam = cameraHolder.GetComponentInChildren<Camera>().transform;

                NetworkServer.Spawn(cameraHolder);
                
                health.onDeath += OnDeath;
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

        private void Update()
        {
            Ray ray = new Ray(cam.position, cam.forward);
            Shoot(ray);
        }

        private void OnDeath()
        {
            StartCoroutine(Respawn());

            cameraHolder.SetActive(false);
            gameObject.SetActive(false);
        }

        private IEnumerator Respawn()
        {
            yield return new WaitForSeconds(2f);

            cameraHolder.SetActive(true);
            gameObject.SetActive(true);
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

        public void Shoot(Ray ray)
        {
            ShootServer(ray);
        }
        
        [Command]
        private void ShootServer(Ray ray)
        {
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 10f))
            {
                Health hitHealth = hit.transform.GetComponent<Health>();

                if (hitHealth != null)
                    hitHealth.Damage(new Damage(10f));
            }
        }
    }
}