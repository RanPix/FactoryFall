using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private float interactionDistance;

    private PlayerControls controls;
    private RaycastHit interact;

    private float removingTimer;
    private Block removedBlock;

    [SerializeField] private float dmg; // test

    private void Awake()
    {
        controls = new PlayerControls();
        controls.Player.Enable();
    }

    private void Update()
    {
        CheckInteraction();
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
                    removedBlock.RemoveBlock();
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
            if (Physics.Raycast(transform.position, transform.forward, out interact, interactionDistance))
                interact.transform.GetComponent<IInteractable>()?.Interact();
        }
        else if (controls.Player.Fire.WasPerformedThisFrame())
        {
            if (Physics.Raycast(transform.position, transform.forward, out interact, interactionDistance))
                interact.transform.GetComponent<IDamagable>()?.Damage(dmg);
        }
    }
}
